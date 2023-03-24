using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;

namespace Luveck.Service.Administration.Repository
{
    public class PatologyRepository : IPatologyRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatologyRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PatologyResponseDto> CreatePatology(PatologyRequestDto patologyDto, string user)
        {
            try
            {
                var patology = await _unitOfWork.PatologyRepository.Find(x => x.Name.ToUpper().Equals(patologyDto.Name.ToUpper()));
                if (patology != null) throw new BusinessException(GeneralMessage.PatologyExist);

                Patology patologyNew = new Patology()
                {
                    Name = patologyDto.Name,
                    CreateBy = user,
                    CreationDate = DateTime.Now,
                    IsDeleted = false,
                    UpdateBy = user,
                    UpdateDate = DateTime.Now,
                };

                await _unitOfWork.PatologyRepository.InsertAsync(patologyNew);

                await _unitOfWork.SaveAsync();

                patology = await _unitOfWork.PatologyRepository.Find(x => x.Name.ToLower().Equals(patologyDto.Name.ToLower()));

                return new PatologyResponseDto()
                {
                    Name = patology.Name,
                    CreateBy = patology.CreateBy, 
                    CreationDate = patology.CreationDate,
                    Id = patology.Id,
                    isDeleted = patology.IsDeleted,
                    UpdateBy = patology.UpdateBy,
                    UpdateDate = patology.UpdateDate,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PatologyResponseDto> UpdatePatology(PatologyRequestDto patologyDto, string user)
        {
            try
            {
                var patology = await _unitOfWork.PatologyRepository.Find(x => x.Id == patologyDto.Id);
                if (patology == null) throw new BusinessException(GeneralMessage.PatologyNoExist);

                if (!patology.Name.ToUpper().Equals(patologyDto.Name.ToUpper()))
                {
                    var pat= await _unitOfWork.PatologyRepository.Find(x => x.Name.ToUpper().Equals(patologyDto.Name.ToUpper()));
                    if (pat != null) throw new BusinessException(GeneralMessage.PatologyExist);
                }
                patology.UpdateDate = DateTime.Now;
                patology.UpdateBy = user;
                patology.IsDeleted = patologyDto.isDeleted;
                patology.Name = patologyDto.Name;

                _unitOfWork.PatologyRepository.Update(patology);                

                await _unitOfWork.SaveAsync();

                patology = await _unitOfWork.PatologyRepository.Find(x => x.Name.ToLower() == patologyDto.Name.ToLower());

                return new PatologyResponseDto()
                {
                    Name = patology.Name,
                    CreateBy = patology.CreateBy,
                    CreationDate = patology.CreationDate,
                    Id = patology.Id,
                    isDeleted = patology.IsDeleted,
                    UpdateBy = patology.UpdateBy,
                    UpdateDate = patology.UpdateDate,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> deletePatology(int id, string user)
        {
            try
            {
                var patology = await _unitOfWork.PatologyRepository.Find(x => x.Id == id);
                if (patology == null) throw new BusinessException(GeneralMessage.PatologyNoExist);

                patology.UpdateDate = DateTime.Now;
                patology.UpdateBy = user;
                patology.IsDeleted = true;

                _unitOfWork.PatologyRepository.Update(patology);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PatologyResponseDto>> GetPatologies()
        {
            List<PatologyResponseDto> lst = await _unitOfWork.PatologyRepository.AsQueryable().Select(x => new PatologyResponseDto
            {
                CreateBy= x.CreateBy,
                CreationDate= DateTime.Now,
                Id= x.Id,
                isDeleted= x.IsDeleted,
                Name= x.Name,
                UpdateBy = x.UpdateBy,
                UpdateDate = x.UpdateDate
            }).ToListAsync();

            return lst;
        }

        public async Task<PatologyResponseDto> GetPatologyById(int id)
        {
            var patology = await _unitOfWork.PatologyRepository.Find(x => x.Id == id);

            return new PatologyResponseDto()
            {
                Name = patology.Name,
                CreateBy = patology.CreateBy,
                CreationDate = patology.CreationDate,
                Id = patology.Id,
                isDeleted = patology.IsDeleted,
                UpdateBy = patology.UpdateBy,
                UpdateDate = patology.UpdateDate,
            };
        }

        public async Task<PatologyResponseDto> GetPatologyByName(string name)
        {
            var patology = await _unitOfWork.PatologyRepository.Find(x => x.Name.ToLower() == name.ToLower());

            return new PatologyResponseDto()
            {
                Name = patology.Name,
                CreateBy = patology.CreateBy,
                CreationDate = patology.CreationDate,
                Id = patology.Id,
                isDeleted = patology.IsDeleted,
                UpdateBy = patology.UpdateBy,
                UpdateDate = patology.UpdateDate,
            };
        }
    }
}
