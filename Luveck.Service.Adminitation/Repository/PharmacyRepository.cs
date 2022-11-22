using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using System;

namespace Luveck.Service.Administration.Repository
{
    public class PharmacyRepository : IPharmacyRepository
    {
        public AppDbContext _db;
        public IMapper _mapper;

        public PharmacyRepository(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PharmacyDto> CreateUpdatePharmacy(PharmacyDto pharmacyDto)
        {
            Pharmacy pharmacy = _mapper.Map<Pharmacy>(pharmacyDto);

            if(pharmacy.Id > 0)
            {
                pharmacy.UpdateDate = DateTime.Now;
                _db.Pharmacy.Update(pharmacy);
            }
            else
            {
                var city = await _db.City.FirstOrDefaultAsync(c => c.Id == pharmacyDto.cityId);
                if (city == null) return null;

                pharmacy.UpdateDate = DateTime.Now;
                pharmacy.CreationDate = DateTime.Now;
                pharmacy.CreateBy = pharmacy.UpdateBy;
                pharmacy.city = city;
                _db.Pharmacy.Add(pharmacy);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<PharmacyDto>(pharmacy);
        }

        public async Task<bool> deletePharmacy(int id, string user)
        {
            try
            {
                Pharmacy pharmacy = await _db.Pharmacy.FirstOrDefaultAsync(c => c.Id == id);

                if (pharmacy == null) return false;

                pharmacy.IsDeleted = true;
                pharmacy.UpdateDate = DateTime.Now;
                pharmacy.UpdateBy = user;
                _db.Pharmacy.Update(pharmacy);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<PharmacyDto>> GetPharmacies()
        {
            return await(from pharmacy in _db.Pharmacy
                         join city in _db.City on pharmacy.city.Id equals city.Id
                         select (new PharmacyDto
                         {
                             Id = pharmacy.Id,
                             Name = pharmacy.Name,
                             Adress = pharmacy.Adress,
                             CreationDate = pharmacy.CreationDate,
                             CreateBy = pharmacy.CreateBy,
                             UpdateDate = pharmacy.UpdateDate,
                             UpdateBy = pharmacy.UpdateBy,
                             IsDeleted = pharmacy.IsDeleted,
                             cityId = city.Id,
                             cityName = city.Name
                         })).ToListAsync();
        }

        public async Task<IEnumerable<PharmacyDto>> GetPharmaciesByCity(int idCity)
        {
            return await(from pharmacy in _db.Pharmacy
                         join city in _db.City on pharmacy.city.Id equals city.Id
                         where pharmacy.city.Id == idCity
                         select (new PharmacyDto
                         {
                             Id = pharmacy.Id,
                             Name = pharmacy.Name,
                             Adress = pharmacy.Adress,
                             CreationDate = pharmacy.CreationDate,
                             CreateBy = pharmacy.CreateBy,
                             UpdateDate = pharmacy.UpdateDate,
                             UpdateBy = pharmacy.UpdateBy,
                             IsDeleted = pharmacy.IsDeleted,
                             cityId = city.Id,
                             cityName = city.Name
                         })).ToListAsync();
        }

        public async Task<IEnumerable<PharmacyDto>> GetPharmaciesByName(string name)
        {
            return await(from pharmacy in _db.Pharmacy
                         join city in _db.City on pharmacy.city.Id equals city.Id
                         where pharmacy.Name.Contains(name) 
                         select (new PharmacyDto
                         {
                             Id = pharmacy.Id,
                             Name = pharmacy.Name,
                             Adress = pharmacy.Adress,
                             CreationDate = pharmacy.CreationDate,
                             CreateBy = pharmacy.CreateBy,
                             UpdateDate = pharmacy.UpdateDate,
                             UpdateBy = pharmacy.UpdateBy,
                             IsDeleted = pharmacy.IsDeleted,
                             cityId = city.Id,
                             cityName = city.Name
                         })).ToListAsync();
        }

        public async Task<PharmacyDto> GetPharmacy(int id)
        {
            return await(from pharmacy in _db.Pharmacy
                         join city in _db.City on pharmacy.city.Id equals city.Id
                         where pharmacy.Id == id
                         select (new PharmacyDto
                         {
                             Id = pharmacy.Id,
                             Name = pharmacy.Name,
                             Adress = pharmacy.Adress,
                             CreationDate = pharmacy.CreationDate,
                             CreateBy = pharmacy.CreateBy,
                             UpdateDate = pharmacy.UpdateDate,
                             UpdateBy = pharmacy.UpdateBy,
                             IsDeleted = pharmacy.IsDeleted,
                             cityId = city.Id,
                             cityName = city.Name
                         })).FirstOrDefaultAsync();
        }
    }
}
