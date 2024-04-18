using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.UnitWork;
using System;
using System.Threading.Tasks;
using System.Linq;
using Luveck.Service.Administration.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Services;
using Microsoft.Extensions.Configuration;
using Luveck.Service.Administration.DTO;

namespace Luveck.Service.Administration.Repository
{
    public class SendPedingExchange : ISendPedingExchange
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly IRestService _restService;
        public SendPedingExchange( IUnitOfWork unitOfWork , IMailService mailService, IConfiguration configuration,
            IRestService restService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _configuration = configuration;
            _restService = restService;
        }

        public async Task<bool> sendMasiveMailRemainder()
        {
            var confirm = await _unitOfWork.MassiveRepository.FirstOrDefaultNoTracking(x => x.sent || !x.sent);
            if (confirm == null)
            {
                confirm = new MassiveRemainder();
                confirm.date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                confirm.sent = true;
                await _unitOfWork.MassiveRepository.InsertAsync(confirm);
                await _unitOfWork.SaveAsync();
            }
            DateTime date = new DateTime( Int32.Parse(confirm.date.Split('-')[0]), Int32.Parse(confirm.date.Split('-')[1]) , Int32.Parse(confirm.date.Split('-')[2]));
            if(DateTime.Now.Subtract(date).TotalDays > 1 || !confirm.sent) 
            {
                var prod = await (from purchase in _unitOfWork.PurchaseRepository.AsQueryable()
                                  join productPurchase in _unitOfWork.ProductPurchaseRepository.AsQueryable() on purchase.Id equals productPurchase.purchaseId
                                  where !productPurchase.Exchanged && !productPurchase.Losed
                                  select new ProductPruchaseByUser()
                                  {
                                      CreationDate = purchase.CreateBy,
                                      IdFactura = purchase.Id,
                                      NoPurchase = purchase.NoPurchase,
                                      productId = productPurchase.productId,
                                      QuantityShiped = productPurchase.QuantityShiped,
                                      UserId = purchase.userId,
                                      DateShiped = purchase.DateShiped
                                  }).ToListAsync();

                List<string> users = prod.Select(p => p.UserId).Distinct().ToList();
                List<string> usersMail = new List<string>();
                if ( prod.Count > 0)
                {
                    foreach (var u in users)
                    {
                        var lstProdu = prod.FindAll(x => x.UserId == u).ToList();
                        foreach (var p in lstProdu)
                        {
                            var rule = await _unitOfWork.ProductChangeRuleRepository.Find(x => x.productId == p.productId && x.state);
                            if (rule == null) continue;
                            var product = await _unitOfWork.ProductRepository.Find(x => x.Id == p.productId);
                            if (product == null) continue;

                            var exhanged = _unitOfWork.ExchangedProductRepository
                                .FindAll(x => x.productId == p.productId && x.userExchanged.ToLower().Equals(p.UserId.ToLower())
                                && x.ExchangeDate.Year == DateTime.Now.Year);
                            int quantityExchanged = 0;
                            foreach (var item in exhanged)
                            {
                                quantityExchanged += item.QuantityExchanged;
                            }
                            if (quantityExchanged >= rule.MaxChangeYear) continue;

                            var productExchange = lstProdu.FindAll(x => x.productId == p.productId ).OrderByDescending(y => y.DateShiped).ToList();
                            int quantity = 0;
                            foreach (var item in productExchange)
                            {
                                quantity += item.QuantityShiped;
                            }
                            if (rule.QuantityBuy > quantity) continue;

                            List<ProductPurchase> ruleApply = new List<ProductPurchase>();
                            ProductPruchaseByUser aux = null;
                            foreach (var item in productExchange)
                            {
                                if (aux == null)
                                {
                                    aux = item;
                                    if (productExchange.Count() > 1) continue;
                                }
                                if (aux.QuantityShiped == rule.QuantityBuy)
                                {
                                    usersMail.Add(u);
                                    break;
                                }

                                if (difDate(aux.DateShiped, item.DateShiped, rule.Periodicity, rule.DaysAround))
                                {
                                    usersMail.Add(u);
                                    break;
                                }
                            }

                            if (usersMail.Exists(x => x == u)) break;
                        }                        
                    }

                    if (usersMail.Count > 0)
                    {
                        await sedMailAsync(usersMail);
                        confirm.date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        confirm.sent = true;
                        _unitOfWork.MassiveRepository.Update(confirm);
                        await _unitOfWork.SaveAsync();
                    }
                }
            }
            return true;
        }

        private bool difDate(DateTime start, DateTime end, int Periodicity, int around)
        {
            double periodicity = (double)Periodicity;
            start = start.AddDays(periodicity);
            DateTime dateStart = new DateTime(start.Year, start.Month, start.Day);
            DateTime dateEnd = new DateTime(end.Year, end.Month, end.Day);
            TimeSpan ts = dateStart.Subtract(dateEnd);
            double total = ts.TotalDays;

            if (ts.TotalDays < 0) total = ts.TotalDays * -1;

            if (total <= around)
                return true;

            return false;
        }

        private async Task sedMailAsync(List<string> users)
        {
            foreach (string user in users) 
            {
                try
                {
                    string url = _configuration.GetSection("NotificationServices:Url").Value;
                    string controller = _configuration.GetSection("NotificationServices:Controller").Value ;
                    Dictionary<string, string> headers = new Dictionary<string, string>
                    { };
                    SendEmailRequestDto request = new SendEmailRequestDto
                    {
                        UrlConfirmar = $"{url}Security?user={user}",
                        user = user
                    };
                    bool result = _restService.PostRestServiceAsync<bool>(url, controller, "SendMail?user=" + user, request, headers).Result;
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
