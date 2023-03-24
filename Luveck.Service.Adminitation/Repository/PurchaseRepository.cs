using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Utils.Resource;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Models;

namespace Luveck.Service.Administration.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IUnitOfWork unitOfWork;
        public PurchaseRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<PurshaseResponseDto> CreatePurchase(PurchaseRequestDto purchaseDto, string user)
        {
            var purchaseExist = await unitOfWork.PurchaseRepository.Find(x => x.NoPurchase.ToUpper().Equals(purchaseDto.NoPurchase.ToUpper()));
            if (purchaseExist != null) throw new BusinessException(GeneralMessage.PurchaseExist);
            var pharmacyExist = await unitOfWork.PharmacyRepository.Find(x => x.Id == purchaseDto.pharmacyId);
            if (pharmacyExist == null) throw new BusinessException(GeneralMessage.PharmacyNoExist);

            Purchase purchase = new Purchase()
            {
                CreateBy = user,
                CreationDate = DateTime.Now,
                NoPurchase = purchaseDto.NoPurchase,
                pharmacyId = pharmacyExist.Id,
                UpdateBy = user,
                UpdateDate = DateTime.Now,
                userId = purchaseDto.userId,
                purchaseReviewed = purchaseDto.Reviewed,
                DateShiped = purchaseDto.DateShiped,
            };

            await unitOfWork.PurchaseRepository.InsertAsync(purchase);
            await unitOfWork.SaveAsync();            

            return await (from pur in unitOfWork.PurchaseRepository.AsQueryable()
                          join pharma in unitOfWork.PharmacyRepository.AsQueryable() on pur.Pharmacy.Id equals pharma.Id
                          join city in unitOfWork.CityRepository.AsQueryable() on pharma.city.Id equals city.Id
                          where pur.Id == purchase.Id
                          select new PurshaseResponseDto()
                          {
                              Id= pur.Id,
                              Buyer = pur.userId,
                              CityPharmacy = city.Name,
                              CreateBy = pur.CreateBy,
                              CreationDate = pur.CreationDate,
                              IdCityPharmacy = city.Id, 
                              UpdateBy = pur.UpdateBy,
                              IdPharmacy = pharma.Id,
                              NamePharmacy = pharma.Name,
                              NoPurchase = pur.NoPurchase,
                              UpdateDate = pur.UpdateDate,
                              Reviewed = pur.purchaseReviewed,
                              DateShiped = pur.DateShiped
                          }).FirstOrDefaultAsync();
        }
        public async Task<PurshaseResponseDto> UpdatePurchase(PurchaseRequestDto purchaseDto, string user)
        {
            var purchaseExist = await unitOfWork.PurchaseRepository.Find(x => x.Id == purchaseDto.Id);
            if (purchaseExist == null) throw new BusinessException(GeneralMessage.PurchaseNoExist);
            var pharmacyExist = await unitOfWork.PharmacyRepository.Find(x => x.Id == purchaseDto.pharmacyId);
            if (pharmacyExist == null) throw new BusinessException(GeneralMessage.PharmacyNoExist);

            if (!purchaseExist.NoPurchase.ToLower().Equals(purchaseDto.NoPurchase.ToLower()))
            {
                var noPurchaseExist = await unitOfWork.PurchaseRepository.Find(x => x.NoPurchase.ToUpper().Equals(purchaseDto.NoPurchase.ToUpper()));
                if (noPurchaseExist != null) throw new BusinessException(GeneralMessage.PurchaseExist);
            }
            purchaseExist.NoPurchase = purchaseDto.NoPurchase;
            purchaseExist.pharmacyId = pharmacyExist.Id;
            purchaseExist.UpdateBy = user;
            purchaseExist.UpdateDate = DateTime.Now;
            purchaseExist.purchaseReviewed = purchaseDto.Reviewed;
            purchaseExist.DateShiped = purchaseDto.DateShiped;

            unitOfWork.PurchaseRepository.Update(purchaseExist);
            await unitOfWork.SaveAsync();

            return await (from pur in unitOfWork.PurchaseRepository.AsQueryable()
                          join pharma in unitOfWork.PharmacyRepository.AsQueryable() on pur.Pharmacy.Id equals pharma.Id
                          join city in unitOfWork.CityRepository.AsQueryable() on pharma.city.Id equals city.Id
                          where pur.Id == purchaseExist.Id
                          select new PurshaseResponseDto()
                          {
                              Id = pur.Id,
                              Buyer = pur.userId,
                              CityPharmacy = city.Name,
                              CreateBy = pur.CreateBy,
                              CreationDate = pur.CreationDate,
                              IdCityPharmacy = city.Id,
                              UpdateBy = pur.UpdateBy,
                              IdPharmacy = pharma.Id,
                              NamePharmacy = pharma.Name,
                              NoPurchase = pur.NoPurchase,
                              UpdateDate = pur.UpdateDate,
                              Reviewed = pur.purchaseReviewed,
                              DateShiped = pur.DateShiped
                          }).FirstOrDefaultAsync();
        }
        public async Task<List<PurshaseResponseDto>> GetPurchases()
        {
            return await (from pur in unitOfWork.PurchaseRepository.AsQueryable()
                                 join pharma in unitOfWork.PharmacyRepository.AsQueryable() on pur.Pharmacy.Id equals pharma.Id
                                 join city in unitOfWork.CityRepository.AsQueryable() on pharma.city.Id equals city.Id
                                 select new PurshaseResponseDto()
                                 {
                                     Id = pur.Id,
                                     Buyer = pur.userId,
                                     CityPharmacy = city.Name,
                                     CreateBy = pur.CreateBy,
                                     CreationDate = pur.CreationDate,
                                     IdCityPharmacy = city.Id,
                                     UpdateBy = pur.UpdateBy,
                                     IdPharmacy = pharma.Id,
                                     NamePharmacy = pharma.Name,
                                     NoPurchase = pur.NoPurchase,
                                     UpdateDate = pur.UpdateDate,
                                     Reviewed = pur.purchaseReviewed,
                                     DateShiped = pur.DateShiped
                                 }).ToListAsync();
        }

        public async Task<List<PurshaseResponseDto>> GetPurchaseByClientID(string userName)
        {
            return await (from pur in unitOfWork.PurchaseRepository.AsQueryable()
                          join pharma in unitOfWork.PharmacyRepository.AsQueryable() on pur.Pharmacy.Id equals pharma.Id
                          join city in unitOfWork.CityRepository.AsQueryable() on pharma.city.Id equals city.Id
                          where pur.userId == userName
                          select new PurshaseResponseDto()
                          {
                              Id = pur.Id,
                              Buyer = pur.userId,
                              CityPharmacy = city.Name,
                              CreateBy = pur.CreateBy,
                              CreationDate = pur.CreationDate,
                              IdCityPharmacy = city.Id,
                              UpdateBy = pur.UpdateBy,
                              IdPharmacy = pharma.Id,
                              NamePharmacy = pharma.Name,
                              NoPurchase = pur.NoPurchase,
                              UpdateDate = pur.UpdateDate,
                              Reviewed = pur.purchaseReviewed,
                              DateShiped = pur.DateShiped
                          }).ToListAsync();
        }

        public async Task<PurshaseResponseDto> GetPurchaseByNoPurchase(string noPurchase)
        {
            return await (from pur in unitOfWork.PurchaseRepository.AsQueryable()
                          join pharma in unitOfWork.PharmacyRepository.AsQueryable() on pur.Pharmacy.Id equals pharma.Id
                          join city in unitOfWork.CityRepository.AsQueryable() on pharma.city.Id equals city.Id
                          where pur.NoPurchase == noPurchase
                          select new PurshaseResponseDto()
                          {
                              Id = pur.Id,
                              Buyer = pur.userId,
                              CityPharmacy = city.Name,
                              CreateBy = pur.CreateBy,
                              CreationDate = pur.CreationDate,
                              IdCityPharmacy = city.Id,
                              UpdateBy = pur.UpdateBy,
                              IdPharmacy = pharma.Id,
                              NamePharmacy = pharma.Name,
                              NoPurchase = pur.NoPurchase,
                              UpdateDate = pur.UpdateDate,
                              Reviewed = pur.purchaseReviewed,
                              DateShiped = pur.DateShiped
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<PurshaseResponseDto>> GetPurchaseByPharmacy(int idPharmacy)
        {
            return await (from pur in unitOfWork.PurchaseRepository.AsQueryable()
                          join pharma in unitOfWork.PharmacyRepository.AsQueryable() on pur.Pharmacy.Id equals pharma.Id
                          join city in unitOfWork.CityRepository.AsQueryable() on pharma.city.Id equals city.Id
                          where pur.pharmacyId == idPharmacy
                          select new PurshaseResponseDto()
                          {
                              Id = pur.Id,
                              Buyer = pur.userId,
                              CityPharmacy = city.Name,
                              CreateBy = pur.CreateBy,
                              CreationDate = pur.CreationDate,
                              IdCityPharmacy = city.Id,
                              UpdateBy = pur.UpdateBy,
                              IdPharmacy = pharma.Id,
                              NamePharmacy = pharma.Name,
                              NoPurchase = pur.NoPurchase,
                              UpdateDate = pur.UpdateDate,
                              Reviewed = pur.purchaseReviewed,
                              DateShiped = pur.DateShiped
                          }).ToListAsync();
        }
    }
}
