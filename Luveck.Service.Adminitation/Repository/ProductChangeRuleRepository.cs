using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.UnitWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Luveck.Service.Administration.Models;
using System;
using Luveck.Service.Administration.Utils.Resource;
using Luveck.Service.Administration.Utils.Exceptions;

namespace Luveck.Service.Administration.Repository
{
    public class ProductChangeRuleRepository : IProductChangeRuleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductChangeRuleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductRuleChangeResponseDto> AddRule(ProductChangeRuleRequestDto request, string userId)
        {
            ProductChangeRule rule = await _unitOfWork.ProductChangeRuleRepository.Find(x=> x.product.Id == request.productId);
            Product prod = await _unitOfWork.ProductRepository.Find(x => x.Id == request.productId);

            if(prod == null) throw new BusinessException(GeneralMessage.ProductNoExist);

            if (rule == null)
            {                
                rule = new ProductChangeRule()
                {
                    CreateBy = userId,
                    CreationDate = DateTime.Now,
                    DaysAround = request.DaysAround,
                    MaxChangeYear= request.MaxChangeYear,
                    Periodicity= request.Periodicity,
                    QuantityBuy= request.QuantityBuy,
                    QuantityGive= request.QuantityGive,
                    state = true,
                    UpdateBy= userId,
                    UpdateDate= DateTime.Now,
                    product = prod
                };

                await _unitOfWork.ProductChangeRuleRepository.InsertAsync(rule);
            }
            else
            {
                rule.DaysAround = request.DaysAround;
                rule.MaxChangeYear = request.MaxChangeYear;
                rule.Periodicity = request.Periodicity;
                rule.QuantityBuy = request.QuantityBuy;
                rule.QuantityGive = request.QuantityGive;
                rule.state = true;
                rule.UpdateBy = userId;
                rule.UpdateDate = DateTime.Now;
                _unitOfWork.ProductChangeRuleRepository.Update(rule);
            }
            await _unitOfWork.SaveAsync();

            return new ProductRuleChangeResponseDto()
            {
                Barcode = rule.product.Barcode,
                CreateBy = rule.CreateBy,
                CreationDate = rule.CreationDate,
                DaysAround = rule.DaysAround,
                Id = rule.Id,
                MaxChangeYear = rule.MaxChangeYear,
                Periodicity = rule.Periodicity,
                productId = rule.product.Id,
                productName = rule.product.Name,
                QuantityBuy = rule.QuantityBuy,
                QuantityGive = rule.QuantityGive,
                state = true,
                UpdateBy = rule.UpdateBy,
                UpdateDate = rule.UpdateDate
            };        
        }

        public async Task<ProductRuleChangeResponseDto> DeleteRule(int id, string userId)
        {
            ProductChangeRule ruleExist = await _unitOfWork.ProductChangeRuleRepository.Find(x => x.Id == id);
            if (ruleExist == null) throw new BusinessException(GeneralMessage.RuleNoExist);
            try
            {
                ruleExist.state = false;
                ruleExist.UpdateDate = DateTime.Now;
                ruleExist.UpdateBy = userId;

                _unitOfWork.ProductChangeRuleRepository.Update(ruleExist);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await (from rule in _unitOfWork.ProductChangeRuleRepository.AsQueryable()
                          join prod in _unitOfWork.ProductRepository.AsQueryable() on rule.product.Id equals prod.Id
                          where rule.Id == id
                          select new ProductRuleChangeResponseDto()
                          {
                              Id = rule.Id,
                              DaysAround = rule.DaysAround,
                              CreateBy = rule.CreateBy,
                              Barcode = prod.Barcode,
                              CreationDate = rule.CreationDate,
                              MaxChangeYear = rule.MaxChangeYear,
                              Periodicity = rule.Periodicity,
                              productId = prod.Id,
                              QuantityBuy = rule.QuantityBuy,
                              QuantityGive = rule.QuantityGive,
                              UpdateBy = rule.UpdateBy,
                              UpdateDate = rule.UpdateDate,
                              state = rule.state,
                              productName = prod.Name
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<ProductRuleChangeResponseDto>> GetRules()
        {
            List < ProductRuleChangeResponseDto > lst = await (from rule in _unitOfWork.ProductChangeRuleRepository.AsQueryable()
                                                         join prod in _unitOfWork.ProductRepository.AsQueryable() on rule.product.Id equals prod.Id
                                                         select new ProductRuleChangeResponseDto()
                                                         {
                                                             Id= rule.Id,
                                                             DaysAround = rule.DaysAround,
                                                             CreateBy = rule.CreateBy,
                                                             Barcode = prod.Barcode,
                                                             CreationDate = rule.CreationDate,
                                                             MaxChangeYear = rule.MaxChangeYear,
                                                             Periodicity = rule.Periodicity,
                                                             productId = prod.Id,
                                                             QuantityBuy = rule.QuantityBuy,
                                                             QuantityGive = rule.QuantityGive,
                                                             UpdateBy = rule.UpdateBy,
                                                             UpdateDate = rule.UpdateDate,
                                                             state = rule.state,
                                                             productName = prod.Name
                                                         }).ToListAsync();
            return lst;
        }

        public async Task<ProductRuleChangeResponseDto> UpdateRule(ProductChangeRuleRequestDto request, string userId)
        {
            ProductChangeRule rule = await _unitOfWork.ProductChangeRuleRepository.Find(x => x.Id == request.Id);
            Product prod = await _unitOfWork.ProductRepository.Find(x => x.Id == request.productId);

            if (prod == null) throw new BusinessException(GeneralMessage.ProductNoExist);

            if (rule == null) throw new BusinessException(GeneralMessage.RuleNoExist); 

            rule.DaysAround = request.DaysAround;
            rule.MaxChangeYear = request.MaxChangeYear;
            rule.Periodicity = request.Periodicity;
            rule.QuantityBuy = request.QuantityBuy;
            rule.QuantityGive = request.QuantityGive;
            rule.state = request.state;
            rule.UpdateBy = userId;
            rule.UpdateDate = DateTime.Now;
            rule.state = request.state;
            
            _unitOfWork.ProductChangeRuleRepository.Update(rule);            
            await _unitOfWork.SaveAsync();

            rule = await _unitOfWork.ProductChangeRuleRepository.Find(x => x.Id == request.Id);

            return new ProductRuleChangeResponseDto()
            {
                Barcode = rule.product.Barcode,
                CreateBy = rule.CreateBy,
                CreationDate = rule.CreationDate,
                DaysAround = rule.DaysAround,
                Id = rule.Id,
                MaxChangeYear = rule.MaxChangeYear,
                Periodicity = rule.Periodicity,
                productId = rule.product.Id,
                productName = rule.product.Name,
                QuantityBuy = rule.QuantityBuy,
                QuantityGive = rule.QuantityGive,
                state = rule.state,
                UpdateBy = rule.UpdateBy,
                UpdateDate = rule.UpdateDate
            };
        }

        public async Task<List<ProductsLandingPageResponseDto>> getProducts()
        {
            return await (from rule in _unitOfWork.ProductChangeRuleRepository.AsQueryable()
                          join prod in _unitOfWork.ProductRepository.AsQueryable() on rule.product.Id equals prod.Id
                          join cat in _unitOfWork.CategoryRepository.AsQueryable() on prod.category.Id equals cat.Id
                          select new ProductsLandingPageResponseDto()
                          {
                              nameProduct = prod.Name,
                              description = prod.Description,
                              urlOfical = prod.UrlOficial,
                              IdCategoria = cat.Id,
                              nameCategoria = cat.Name
                          }).ToListAsync();
        }

        public async Task<ProductRuleChangeResponseDto> GetRuleById(int id)
        {
            return await(from rule in _unitOfWork.ProductChangeRuleRepository.AsQueryable()
                         join prod in _unitOfWork.ProductRepository.AsQueryable() on rule.product.Id equals prod.Id
                         where rule.Id == id
                         select new ProductRuleChangeResponseDto()
                         {
                             Id = rule.Id,
                             DaysAround = rule.DaysAround,
                             CreateBy = rule.CreateBy,
                             Barcode = prod.Barcode,
                             CreationDate = rule.CreationDate,
                             MaxChangeYear = rule.MaxChangeYear,
                             Periodicity = rule.Periodicity,
                             productId = prod.Id,
                             QuantityBuy = rule.QuantityBuy,
                             QuantityGive = rule.QuantityGive,
                             UpdateBy = rule.UpdateBy,
                             UpdateDate = rule.UpdateDate,
                             state = rule.state,
                             productName = prod.Name
                         }).FirstOrDefaultAsync();
        }
    }
}
