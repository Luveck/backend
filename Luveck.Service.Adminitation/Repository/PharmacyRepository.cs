using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;
using Luveck.Service.Administration.DTO;

namespace Luveck.Service.Administration.Repository
{
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PharmacyRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public async Task<PharmacyResponseDto> UpdatePharmacy(PharmacyRequestDto pharmacyDto, string user)
        {
            Pharmacy pharmacy;
            try
            {
                pharmacy = await _unitOfWork.PharmacyRepository.FirstOrDefaultNoTracking(x => x.Id == pharmacyDto.Id);
                if (pharmacy == null) throw new BusinessException(GeneralMessage.PharmacyNoExist);
                City city = await _unitOfWork.CityRepository.FirstOrDefaultNoTracking(x => x.Id == pharmacyDto.cityId);
                if (city == null) throw new BusinessException(GeneralMessage.CityNoExist);

                if (!pharmacy.Name.ToLower().Equals(pharmacyDto.Name.ToLower()))
                {
                    var pharmacyExist = await _unitOfWork.PharmacyRepository.FirstOrDefaultNoTracking(x => x.Name.ToUpper().Equals(pharmacyDto.Name));
                    if (pharmacyExist != null) throw new BusinessException(GeneralMessage.PharmacyExist);
                }

                pharmacy.UpdateDate = DateTime.Now;
                pharmacy.UpdateBy = user;
                pharmacy.Adress = pharmacyDto.Adress;
                pharmacy.Name = pharmacyDto.Name;
                pharmacy.IsDeleted = pharmacyDto.IsDeleted;
                pharmacy.city = city;

                _unitOfWork.PharmacyRepository.Update(pharmacy);
                await _unitOfWork.SaveAsync();

                return await (from ph in _unitOfWork.PharmacyRepository.AsQueryable()
                              join cit in _unitOfWork.CityRepository.AsQueryable() on ph.city.Id equals cit.Id
                              where ph.Id == pharmacy.Id
                              select new PharmacyResponseDto()
                              {
                                  Adress = ph.Adress,
                                  cityId = ph.city.Id,
                                  city = cit.Name,
                                  CreateBy = ph.CreateBy,
                                  CreationDate = ph.CreationDate,
                                  Id = ph.Id,
                                  IsDeleted = ph.IsDeleted,
                                  Name = ph.Name,
                                  UpdateBy = ph.UpdateBy,
                                  UpdateDate = ph.UpdateDate
                              }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PharmacyResponseDto> CreatePharmacy(PharmacyRequestDto pharmacyDto, string user)
        {
            Pharmacy pharmacy;
            try
            {
                pharmacy = await _unitOfWork.PharmacyRepository.FirstOrDefaultNoTracking(x => x.Name.ToUpper() == pharmacyDto.Name.ToUpper());
                if (pharmacy != null) throw new BusinessException(GeneralMessage.PharmacyExist);
                City city = await _unitOfWork.CityRepository.FirstOrDefaultNoTracking(x => x.Id == pharmacyDto.cityId);
                if (city == null) throw new BusinessException(GeneralMessage.CityNoExist);

                pharmacy = new Pharmacy();
                pharmacy.UpdateDate = DateTime.Now;
                pharmacy.UpdateBy = user;
                pharmacy.CreationDate = DateTime.Now;
                pharmacy.CreateBy = user;
                pharmacy.Adress = pharmacyDto.Adress;
                pharmacy.Name = pharmacyDto.Name;
                pharmacy.IsDeleted = false;
                pharmacy.cityId = city.Id;

                await _unitOfWork.PharmacyRepository.InsertAsync(pharmacy);
                await _unitOfWork.SaveAsync();

                return await (from ph in _unitOfWork.PharmacyRepository.AsQueryable()
                              join cit in _unitOfWork.CityRepository.AsQueryable() on ph.city.Id equals cit.Id
                              where ph.Name.ToUpper().Equals(pharmacyDto.Name.ToUpper())
                              select new PharmacyResponseDto()
                              {
                                  Adress = ph.Adress,
                                  cityId = ph.city.Id,
                                  city = cit.Name,
                                  CreateBy = ph.CreateBy,
                                  CreationDate = ph.CreationDate,
                                  Id = ph.Id,
                                  IsDeleted = ph.IsDeleted,
                                  Name = ph.Name,
                                  UpdateBy = ph.UpdateBy,
                                  UpdateDate = ph.UpdateDate
                              }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> deletePharmacy(int id, string user)
        {
            try
            {
                Pharmacy pharmacy = await _unitOfWork.PharmacyRepository.FirstOrDefaultNoTracking(x => x.Id == id);

                if (pharmacy == null) throw new BusinessException(GeneralMessage.PharmacyNoExist);

                pharmacy.IsDeleted = true;
                pharmacy.UpdateDate = DateTime.Now;
                pharmacy.UpdateBy = user;

                _unitOfWork.PharmacyRepository.Update(pharmacy);
                await _unitOfWork.SaveAsync();
 
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PharmacyResponseDto>> GetPharmacies()
        {
            List<PharmacyResponseDto> lst = await (from pharmacy in _unitOfWork.PharmacyRepository.AsQueryable()
                                                   join city in _unitOfWork.CityRepository.AsQueryable() on pharmacy.city.Id equals city.Id
                                                   select new PharmacyResponseDto()
                                                   {
                                                       Name = pharmacy.Name,
                                                       Adress = pharmacy.Adress,
                                                       cityId = city.Id,
                                                       CreateBy = pharmacy.CreateBy,
                                                       CreationDate = pharmacy.CreationDate,
                                                       IsDeleted = pharmacy.IsDeleted,
                                                       Id = pharmacy.Id,
                                                       city = city.Name,
                                                       UpdateBy = pharmacy.UpdateBy,
                                                       UpdateDate = pharmacy.UpdateDate
                                                   }).ToListAsync();

            return lst;
        }

        public async Task<List<PharmacyResponseDto>> GetPharmaciesByCity(int idCity)
        {
            List<PharmacyResponseDto> lst = await (from pharmacy in _unitOfWork.PharmacyRepository.AsQueryable()
                                                   join city in _unitOfWork.CityRepository.AsQueryable() on pharmacy.city.Id equals city.Id
                                                   where pharmacy.city.Id == idCity
                                                   select new PharmacyResponseDto()
                                                   {
                                                       Name = pharmacy.Name,
                                                       Adress = pharmacy.Adress,
                                                       cityId = city.Id,
                                                       CreateBy = pharmacy.CreateBy,
                                                       CreationDate = pharmacy.CreationDate,
                                                       IsDeleted = pharmacy.IsDeleted,
                                                       Id = pharmacy.Id,
                                                       city = city.Name,
                                                       UpdateBy = pharmacy.UpdateBy,
                                                       UpdateDate = pharmacy.UpdateDate
                                                   }).ToListAsync();

            return lst;
        }

        public async Task<List<PharmacyResponseDto>> GetPharmaciesByName(string name)
        {
            List<PharmacyResponseDto> lst = await (from pharmacy in _unitOfWork.PharmacyRepository.AsQueryable()
                                                   join city in _unitOfWork.CityRepository.AsQueryable() on pharmacy.city.Id equals city.Id
                                                   where pharmacy.Name.ToLower().Contains(name.ToLower())
                                                   select new PharmacyResponseDto()
                                                   {
                                                       Name= pharmacy.Name,
                                                       Adress = pharmacy.Adress,
                                                       cityId= city.Id,
                                                       CreateBy= pharmacy.CreateBy,
                                                       CreationDate= pharmacy.CreationDate,
                                                       IsDeleted= pharmacy.IsDeleted,
                                                       Id = pharmacy.Id,
                                                       city = city.Name,
                                                       UpdateBy = pharmacy.UpdateBy,
                                                       UpdateDate = pharmacy.UpdateDate
                                                   }).ToListAsync();

            return lst;
        }

        public async Task<PharmacyResponseDto> GetPharmacy(int id)
        {
            return await (from pharmacy in _unitOfWork.PharmacyRepository.AsQueryable()
                          join city in _unitOfWork.CityRepository.AsQueryable() on pharmacy.city.Id equals city.Id
                          where pharmacy.Id == id
                          select new PharmacyResponseDto()
                          {
                              Name = pharmacy.Name,
                              Adress = pharmacy.Adress,
                              cityId = city.Id,
                              CreateBy = pharmacy.CreateBy,
                              CreationDate = pharmacy.CreationDate,
                              IsDeleted = pharmacy.IsDeleted,
                              Id = pharmacy.Id,
                              city = city.Name,
                              UpdateBy = pharmacy.UpdateBy,
                              UpdateDate = pharmacy.UpdateDate
                          }).FirstOrDefaultAsync();
        }
    }
}
