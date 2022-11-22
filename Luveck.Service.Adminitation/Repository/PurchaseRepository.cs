using AutoMapper;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Luveck.Service.Administration.Data;

namespace Luveck.Service.Administration.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;

        public PurchaseRepository(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PurchaseDto> CreatePurchase(PurchaseDto purchaseDto)
        {
            Purchase purchase = _mapper.Map<Purchase>(purchaseDto);
            var pharmacy = await _db.Pharmacy.FirstOrDefaultAsync(p => p.Id == purchaseDto.PharmacyId);
            if (pharmacy == null) return null;
            var user = await _db.User.FirstOrDefaultAsync(u => u.UserName == purchaseDto.NameUser);
            if(user == null) return null;

            purchase.UpdateDate = DateTime.Now;
            purchase.CreationDate = DateTime.Now;
            purchase.CreateBy = purchaseDto.CreateBy;
            purchase.UpdateBy = purchaseDto.CreateBy;
            purchase.Pharmacy = pharmacy;
            purchase.user = user;
            _db.Purchase.Add(purchase);
            await _db.SaveChangesAsync();
 
            return _mapper.Map<PurchaseDto>(purchase);
        }

        public async Task<IEnumerable<PurchaseDto>> GetPurchases()
        {
            return await(from Purchase in _db.Purchase
                         join pharmacy in _db.Pharmacy on Purchase.Pharmacy.Id equals pharmacy.Id
                         join user in _db.User on Purchase.user.UserName equals user.UserName
                         select (new PurchaseDto
                         {
                             Id = Purchase.Id,
                             NoPurchase = Purchase.NoPurchase,
                             PharmacyId = pharmacy.Id,
                             PharmacyName = pharmacy.Name,
                             CreateBy = Purchase.CreateBy,
                             CreationDate = Purchase.CreationDate,
                             UpdateBy = Purchase.UpdateBy,
                             UpdateDate = Purchase.UpdateDate,
                             NameUser = user.Name,
                             userId = user.UserName,
                         })).ToListAsync();
        }

        public async Task<IEnumerable<PurchaseDto>> GetPurchaseByClientID(string userName)
        {
            return await(from Purchase in _db.Purchase
                         join pharmacy in _db.Pharmacy on Purchase.Pharmacy.Id equals pharmacy.Id
                         join user in _db.User on Purchase.user.UserName equals user.UserName
                         where Purchase.user.UserName == userName
                         select (new PurchaseDto
                         {
                             Id = Purchase.Id,
                             NoPurchase = Purchase.NoPurchase,
                             PharmacyId = pharmacy.Id,
                             PharmacyName = pharmacy.Name,
                             CreateBy = Purchase.CreateBy,
                             CreationDate = Purchase.CreationDate,
                             UpdateBy = Purchase.UpdateBy,
                             UpdateDate = Purchase.UpdateDate,
                             NameUser = user.Name,
                             userId = user.UserName,
                         })).ToListAsync();
        }

        public async Task<PurchaseDto> GetPurchaseByNoPurchase(string noPurchase)
        {
            return await(from Purchase in _db.Purchase
                         join pharmacy in _db.Pharmacy on Purchase.Pharmacy.Id equals pharmacy.Id
                         join user in _db.User on Purchase.user.UserName equals user.UserName
                         where Purchase.NoPurchase == noPurchase
                         select (new PurchaseDto
                         {
                             Id = Purchase.Id,
                             NoPurchase = Purchase.NoPurchase,
                             PharmacyId = pharmacy.Id,
                             PharmacyName = pharmacy.Name,
                             CreateBy = Purchase.CreateBy,
                             CreationDate = Purchase.CreationDate,
                             UpdateBy = Purchase.UpdateBy,
                             UpdateDate = Purchase.UpdateDate,
                             NameUser = user.Name,
                             userId = user.UserName,
                         })).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PurchaseDto>> GetPurchaseByPharmacy(int idPharmacy)
        {
            return await(from Purchase in _db.Purchase
                         join pharmacy in _db.Pharmacy on Purchase.Pharmacy.Id equals pharmacy.Id 
                         join user in _db.User on Purchase.user.UserName equals user.UserName
                         where Purchase.Pharmacy.Id == idPharmacy
                         select (new PurchaseDto
                         {
                             Id = Purchase.Id,
                             NoPurchase = Purchase.NoPurchase,
                             PharmacyId = pharmacy.Id,
                             PharmacyName = pharmacy.Name,
                             CreateBy = Purchase.CreateBy,
                             CreationDate = Purchase.CreationDate,
                             UpdateBy = Purchase.UpdateBy,
                             UpdateDate = Purchase.UpdateDate,
                             NameUser = user.Name,
                             userId = user.UserName,
                         })).ToListAsync();
        }
    }
}
