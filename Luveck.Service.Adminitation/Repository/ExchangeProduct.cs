using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.UnitWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository
{
    public class ExchangeProduct : IExchangeProduct
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExchangeProduct(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public async Task<List<ExchangeProductAvailableResponseDto>> getProductExchangeAvailable(string user)
        {
            return await (from prodPurchase in _unitOfWork.ProductPurchaseRepository.AsQueryable()
                          join purchase in _unitOfWork.PurchaseRepository.AsQueryable() on prodPurchase.Purchase.Id equals purchase.Id
                          join prod in _unitOfWork.ProductRepository.AsQueryable() on prodPurchase.Product.Id equals prod.Id
                          where purchase.userId == user
                          select new ExchangeProductAvailableResponseDto()
                          {
                              ProductId = prod.Id,
                              ProductName = prod.Name
                          }).ToListAsync();


        }
        public async Task<List<ExchangeResponseDto>> ExchangeProduc(List<ExchangeRequestDto> request, string user)
        {
            List < ExchangeResponseDto > response = new List < ExchangeResponseDto >();
            try
            {
                foreach (var prod in request)
                {
                    var rule = await _unitOfWork.ProductChangeRuleRepository.Find(x => x.productId == prod.productId && x.state);
                    var product = await _unitOfWork.ProductRepository.Find(x => x.Id == prod.productId);
                    if( product == null)
                    {
                        response.Add(new ExchangeResponseDto
                        {
                            Exchaged = false,
                            ProductId = prod.productId,
                            ProductName = "no existe",
                            Response = "Producto no existe."
                        });
                        continue;
                    }
                    if (rule == null)
                    {
                        response.Add(new ExchangeResponseDto
                        {
                            Exchaged = false,
                            ProductId = prod.productId,
                            ProductName = product.Name,
                            Response = "Producto no aplica para canje o no tiene activa la promoción."
                        });
                        continue;
                    }

                    var exhanged = _unitOfWork.ExchangedProductRepository
                        .FindAll(x => x.productId == prod.productId && x.userExchanged.ToLower().Equals(prod.user.ToLower())
                        && x.ExchangeDate.Year == DateTime.Now.Year);
                    
                    int quantityExchanged = 0;
                    foreach (var item in exhanged)
                    {
                       quantityExchanged += item.QuantityExchanged;
                    }
                    if(quantityExchanged >= rule.MaxChangeYear)
                    {
                        response.Add(new ExchangeResponseDto
                        {
                            Exchaged = false,
                            ProductId = prod.productId,
                            ProductName = product.Name,
                            Response = "Producto no aplica para canje, ya cumplio el máximo del año."
                        });
                        continue;
                    }
                    var productExchange = _unitOfWork.ProductPurchaseRepository
                        .FindAll(x => x.Product.Id == prod.productId && x.Losed == false && x.Exchanged == false).OrderByDescending(y=> y.DateShiped);
                    int quantity = 0;
                    foreach (var item in productExchange)
                    {
                        quantity += item.QuantityShiped;
                    }
                    if(rule.QuantityBuy > quantity)
                    {
                        response.Add(new ExchangeResponseDto
                        {
                            Exchaged = false,
                            ProductId = prod.productId,
                            ProductName= product.Name,
                            Response = "Producto no ha comprado las suficientes unidades para el canje."
                        });
                        continue;
                    }
                    
                    List<ProductPurchase> prudExchangeReviewed = new List<ProductPurchase>();
                    List<ProductPurchase> ruleApply = new List<ProductPurchase>();
                    ProductPurchase aux = null;
                    int cantExchange = 0;
                    int prodExchanged = 0;
                    foreach (var item in productExchange)
                    {
                        if(aux == null)
                        {
                            aux = item;
                            if(productExchange.Count() > 1) continue;
                        }
                        if(aux.QuantityShiped == rule.QuantityBuy)
                        {
                            aux.Exchanged = true;
                            prudExchangeReviewed.Add(aux);
                            prodExchanged += 1;
                            aux = null;
                            continue;
                        }
                        
                        if(difDate(aux.DateShiped, item.DateShiped, rule.Periodicity, rule.DaysAround))
                        {
                            aux.Exchanged = true;
                            ruleApply.Add(aux);
                            cantExchange += aux.QuantityShiped;
                            aux = item;
                            if (productExchange.Last().Id == item.Id && cantExchange < rule.QuantityBuy)
                            {
                                item.Exchanged = true;
                                ruleApply.Add(item);
                                cantExchange += item.QuantityShiped;
                            }
                        }
                        else
                        {
                            if(aux.Id != item.Id)
                            {
                                aux.Losed = true;
                                _unitOfWork.ProductPurchaseRepository.Update(aux);
                            }
                        }
                        if(cantExchange >= rule.QuantityBuy)
                        {
                            foreach (var add in ruleApply)
                            {
                                prudExchangeReviewed.Add(add);
                            }
                            prodExchanged += 1;
                            cantExchange = 0;
                        }
                    }

                    if(prudExchangeReviewed.Count() > 0)
                    {
                        _unitOfWork.ProductPurchaseRepository.Update(prudExchangeReviewed);
                    }

                    response.Add(new ExchangeResponseDto
                    {
                        Exchaged = true,
                        ProductId = prod.productId,
                        ProductName = product.Name,
                        Response = "Productos canjeados " + prodExchanged
                    });
                    await _unitOfWork.ExchangedProductRepository.InsertAsync(new ExchangedProduct()
                    {
                        ExchangeBy = user,
                        ExchangeDate = DateTime.Now,
                        productId = prod.productId,
                        QuantityExchanged = prodExchanged,
                        userExchanged = prod.user,
                    });
                }
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        private bool difDate(DateTime start, DateTime end, int Periodicity, int around)
        {
            double periodicity = (double)Periodicity;
            start = start.AddDays(periodicity);
            DateTime dateStart = new DateTime(start.Year, start.Month, start.Day);
            DateTime dateEnd = new DateTime(end.Year, end.Month, end.Day);
            TimeSpan ts = dateStart.Subtract(dateEnd);
            double total = ts.TotalDays;

            if(ts.TotalDays < 0) total = ts.TotalDays * -1;

            if (total <= around)
                return true;

            return false;
        }
    }
}
