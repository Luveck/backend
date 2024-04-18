using Luveck.Service.Administration.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using System;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.DTO;
using System.Collections.Generic;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Luveck.Service.Administration.Repository
{
    public class MedicalRespository : IMedicalRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public MedicalRespository(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public async Task<MedicalResponseDto> CreateMedical(MedicalRequestDto medicalDto, string user)
        {
            try
            {
                var patology = await _unitOfWork.PatologyRepository.Find(x => x.Id == medicalDto.patologyId);
                if(patology == null) throw new BusinessException(GeneralMessage.PatologyNoExist);
                var medical = await _unitOfWork.MedicalRepository.Find(x => x.register.ToLower() == medicalDto.register.ToLower());
                if (medical != null) throw new BusinessException(GeneralMessage.MedicalExist);

                Medical medic = new Medical()
                {
                    Name = medicalDto.Name,
                    register = medicalDto.register,
                    CreateBy = user,
                    CreationDate = DateTime.Now,
                    isDeleted = false,
                    UpdateBy = user,
                    UpdateDate = DateTime.Now,
                    patologyId = patology.Id
                };
                await _unitOfWork.MedicalRepository.InsertAsync(medic);
                await _unitOfWork.SaveAsync();

                medical = await _unitOfWork.MedicalRepository.Find(x => x.register.ToLower() == medicalDto.register.ToLower());
                return new MedicalResponseDto()
                {
                    CreateBy= medical.CreateBy,
                    CreationDate = medical.CreationDate,
                    Id=medical.Id,
                    isDeleted= medical.isDeleted,
                    Name = medical.Name,
                    patologyId = patology.Id,
                    patologyName = patology.Name,
                    register = medical.register,
                    UpdateBy = medical.UpdateBy,
                    UpdateDate = medical.UpdateDate
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<MedicalResponseDto> UpdateMedical(MedicalRequestDto medicalDto, string user)
        {
            try
            {
                var patology = await _unitOfWork.PatologyRepository.Find(x => x.Id == medicalDto.patologyId);
                if (patology == null) throw new BusinessException(GeneralMessage.PatologyNoExist);
                var medical = await _unitOfWork.MedicalRepository.Find(x => x.Id == medicalDto.Id);
                if (medical == null) throw new BusinessException(GeneralMessage.MedicalNoExist);

                if (!medical.register.ToLower().Equals(medicalDto.register.ToLower()))
                {
                    var registerExist = await _unitOfWork.MedicalRepository.Find(x => x.register.ToLower().Equals(medicalDto.register.ToLower()));
                    if (registerExist != null) throw new BusinessException(GeneralMessage.MedicalNoExist);
                }

                medical.patologyId = patology.Id;
                medical.Name = medicalDto.Name;
                medical.register = medicalDto.register;
                medical.UpdateBy = user;
                medical.UpdateDate = DateTime.Now;
                medical.isDeleted = medicalDto.isDeleted;

                _unitOfWork.MedicalRepository.Update(medical);                
                await _unitOfWork.SaveAsync();

                return new MedicalResponseDto()
                {
                    CreateBy = medical.CreateBy,
                    CreationDate = medical.CreationDate,
                    Id = medical.Id,
                    isDeleted = medical.isDeleted,
                    Name = medical.Name,
                    patologyId = patology.Id,
                    patologyName = patology.Name,
                    register = medical.register,
                    UpdateBy = medical.UpdateBy,
                    UpdateDate = medical.UpdateDate
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> deleteMedical(int id, string user)
        {
            try
            {
                var medical = await _unitOfWork.MedicalRepository.Find(x => x.Id == id);
                if(medical == null) throw new BusinessException(GeneralMessage.MedicalNoExist);
                medical.isDeleted = true;

                _unitOfWork.MedicalRepository.Update(medical);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MedicalResponseDto> GetMedical(int id)
        {
            return await (from med in _unitOfWork.MedicalRepository.AsQueryable()
                          join pat in _unitOfWork.PatologyRepository.AsQueryable() on med.patology.Id equals pat.Id
                          where med.Id == id
                          select new MedicalResponseDto()
                          {
                              CreateBy = med.CreateBy,
                              CreationDate = med.CreationDate,
                              Id = med.Id,
                              isDeleted = med.isDeleted,
                              Name = med.Name,
                              patologyId = pat.Id,
                              patologyName = pat.Name,
                              register = med.register,
                              UpdateBy = med.UpdateBy,
                              UpdateDate = med.UpdateDate
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<MedicalResponseDto>> GetMedicalByName(string name)
        {
            List<MedicalResponseDto> lst = await (from med in _unitOfWork.MedicalRepository.AsQueryable()
                                                  join pat in _unitOfWork.PatologyRepository.AsQueryable() on med.patology.Id equals pat.Id
                                                  where med.Name.ToLower().Contains(name.ToLower())
                                                  select new MedicalResponseDto
                                                  {
                                                      CreateBy= med.CreateBy,
                                                      Name= med.Name,
                                                      CreationDate= med.CreationDate,
                                                      Id= med.Id,
                                                      isDeleted = med.isDeleted,
                                                      patologyId = pat.Id,
                                                      patologyName = pat.Name,
                                                      register = med.register, 
                                                      UpdateBy = med.UpdateBy,
                                                      UpdateDate = med.UpdateDate
                                                  }).ToListAsync();
            return lst;
        }

        public async Task<List<MedicalResponseDto>> GetMedicalByPatolgy(int idPatology)
        {
            List<MedicalResponseDto> lst = await(from med in _unitOfWork.MedicalRepository.AsQueryable()
                                                 join pat in _unitOfWork.PatologyRepository.AsQueryable() on med.patology.Id equals pat.Id
                                                 where pat.Id == idPatology
                                                 select new MedicalResponseDto
                                                 {
                                                     CreateBy = med.CreateBy,
                                                     Name = med.Name,
                                                     CreationDate = med.CreationDate,
                                                     Id = med.Id,
                                                     isDeleted = med.isDeleted,
                                                     patologyId = pat.Id,
                                                     patologyName = pat.Name,
                                                     register = med.register,
                                                     UpdateBy = med.UpdateBy,
                                                     UpdateDate = med.UpdateDate
                                                 }).ToListAsync();
            return lst;
        }

        public async Task<List<MedicalResponseDto>> GetMedicals()
        {
            List<MedicalResponseDto> lst = await(from med in _unitOfWork.MedicalRepository.AsQueryable()
                                                 join pat in _unitOfWork.PatologyRepository.AsQueryable() on med.patology.Id equals pat.Id
                                                 select new MedicalResponseDto
                                                 {
                                                     CreateBy = med.CreateBy,
                                                     Name = med.Name,
                                                     CreationDate = med.CreationDate,
                                                     Id = med.Id,
                                                     isDeleted = med.isDeleted,
                                                     patologyId = pat.Id,
                                                     patologyName = pat.Name,
                                                     register = med.register,
                                                     UpdateBy = med.UpdateBy,
                                                     UpdateDate = med.UpdateDate
                                                 }).ToListAsync();
            return lst;
        }
    }
}
