using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;
using System.Linq;
using Luveck.Service.Administration.Models;
using System;
using Luveck.Service.Administration.DTO.Response;
using Microsoft.EntityFrameworkCore;

namespace Luveck.Service.Administration.Repository
{
    public class ProductPurchaseRepository : IProductPurchaseRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductPurchaseRepository( IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public async Task<string> CreateUpdateProductPurchase(ProductPurchaseRequestDto request, string user)
        {
            try
            {
                var purchase = await _unitOfWork.PurchaseRepository.Find(x => x.Id == request.purchaseId);
                if (purchase == null) throw new BusinessException(GeneralMessage.PurchaseNoExist);

                List<ProductPurchase> lstProductPurchase = _unitOfWork.ProductPurchaseRepository.FindAll(x => x.purchaseId == purchase.Id).ToList();
                if (lstProductPurchase.Count > 0)
                {
                    List<ProductPurchase> lstToDelete = lstProductPurchase.FindAll(x => x.Exchanged == false && x.Losed == false);
                    _unitOfWork.ProductPurchaseRepository.Delete(lstToDelete);

                    List<ProductPurchase> lst = new List<ProductPurchase>();

                    foreach (var item in request.ProductPurchase)
                    {
                        var prod = lstProductPurchase.Find(x => x.productId == item.productId);
                        if (prod != null)
                        {
                            if (prod.Exchanged || prod.Losed) continue;

                            prod.UpdateBy = user;
                            prod.UpdateDate = DateTime.Now;
                            prod.DateShiped = item.DateShiped;
                            prod.QuantityShiped = item.QuantityShiped;
                            lst.Add(prod);
                        }
                        else
                        {
                            var pro = await _unitOfWork.ProductRepository.Find(x => x.Id == item.productId);
                            if (pro == null) continue;
                            lst.Add(new ProductPurchase()
                            {
                                Id = string.Concat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,
                                DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond),
                                purchaseId = purchase.Id,
                                CreateBy = user,
                                CreationDate = DateTime.Now,
                                DateShiped = item.DateShiped,
                                Exchanged = false,
                                Losed = false,
                                productId = pro.Id,
                                QuantityShiped = item.QuantityShiped,
                                UpdateBy = user,
                                UpdateDate = DateTime.Now,
                            });
                        }
                    }
                    if (lst.Count > 0) await _unitOfWork.ProductPurchaseRepository.InsertRangeAsync(lst);
                    else return null;
                }
                else
                {
                    List<ProductPurchase> lst = new List<ProductPurchase>();
                    foreach (var product in request.ProductPurchase)
                    {
                        var pro = await _unitOfWork.ProductRepository.Find(x => x.Id == product.productId);
                        if (pro == null) continue;
                        lst.Add(new ProductPurchase()
                        {
                            Id = string.Concat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,
                                DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond),
                            purchaseId = purchase.Id,
                            CreateBy = user,
                            CreationDate = DateTime.Now,
                            DateShiped = product.DateShiped,
                            Exchanged = false,
                            Losed = false,
                            productId = pro.Id,
                            QuantityShiped = product.QuantityShiped,
                            UpdateBy = user,
                            UpdateDate = DateTime.Now,
                        });
                    }
                    if (lst.Count > 0) await _unitOfWork.ProductPurchaseRepository.InsertRangeAsync(lst);
                    else return null;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "200";
        }

        public async Task<bool> DeleteProductsPurchase(ProductPurchaseRequestDto request, string user)
        {
            try
            {
                var purchase = await _unitOfWork.PurchaseRepository.Find(x => x.Id == request.purchaseId);
                if (purchase == null) throw new BusinessException(GeneralMessage.PurchaseNoExist);

                List<ProductPurchase> lstProductPurchase = _unitOfWork.ProductPurchaseRepository
                    .FindAll(x => x.purchaseId == purchase.Id && x.Losed == false && x.Exchanged == false).ToList();

                List<ProductPurchase> lstDelete = new List<ProductPurchase>();

                foreach (var item in request.ProductPurchase)
                {
                    var prod = lstProductPurchase.Find(x => x.productId == item.productId);
                    if (prod != null) lstDelete.Add(prod);
                }

                _unitOfWork.ProductPurchaseRepository.Delete(lstDelete);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProductPruchaseResponseDto>> getProductsByPurchase(int purchaseId)
        {
            return await (from purchase in _unitOfWork.ProductPurchaseRepository.AsQueryable()
                          join product in _unitOfWork.ProductRepository.AsQueryable() on purchase.Product.Id equals product.Id
                          where purchase.purchaseId == purchaseId 
                          select new ProductPruchaseResponseDto()
                          {
                              CreateBy = purchase.CreateBy,
                              CreationDate = purchase.CreationDate,
                              DateShiped = purchase.DateShiped,
                              ProductId = product.Id,
                              ProductName = product.Name,
                              QuantityShiped = purchase.QuantityShiped,
                              UpdateBy = purchase.UpdateBy,
                              UpdateDate = purchase.UpdateDate
                          }).ToListAsync();
        }
    }
}
