using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public class MedicalRespository : IMedicalRepository
    {
        public AppDbContext _db;
        public IMapper _mapper;

        public MedicalRespository(AppDbContext appDb, IMapper mapper)
        {
            _db = appDb;
            _mapper = mapper;
        }
        public async Task<MedicalDto> CreateUpdateMedical(MedicalDto medicalDto)
        {
            Medical medical = _mapper.Map<Medical>(medicalDto);

            if (medical.Id > 0)
            {
                _db.Medical.Update(medical);
            }
            else
            {
                var patology = await _db.Patology.FirstOrDefaultAsync(c => c.Id == medicalDto.patologyId);
                if (patology == null) return null;

                medical.UpdateDate = DateTime.Now;
                medical.CreationDate = DateTime.Now;
                medical.CreateBy = medical.UpdateBy;
                medical.patology = patology;
                _db.Medical.Add(medical);
            }

            await _db.SaveChangesAsync();

            return _mapper.Map<MedicalDto>(medical);
        }

        public async Task<bool> deleteMedical(int id, string user)
        {
            try
            {
                Medical medical = await _db.Medical.FirstOrDefaultAsync(c => c.Id == id);

                if (medical == null) return false;

                medical.isDeleted = true;
                medical.UpdateDate = DateTime.Now;
                medical.UpdateBy = user;
                _db.Medical.Update(medical);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<MedicalDto> GetMedical(int id)
        {
            return await(from medical in _db.Medical
                         join patology in _db.Patology on medical.patology.Id equals patology.Id
                         where medical.Id == id
                         select (new MedicalDto
                         {
                             Id = medical.Id,
                             Name = medical.Name,
                             register = medical.register,
                             CreateBy = medical.CreateBy,
                             CreationDate = medical.CreationDate,
                             isDeleted = medical.isDeleted,
                             patologyId = patology.Id,
                             patologyName = patology.Name,
                             UpdateBy = medical.UpdateBy,
                             UpdateDate = medical.UpdateDate,
                         })).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MedicalDto>> GetMedicalByName(string name)
        {
            return await(from medical in _db.Medical
                         join patology in _db.Patology on medical.patology.Id equals patology.Id
                         where medical.Name.Contains(name)
                         select (new MedicalDto
                         {
                             Id = medical.Id,
                             Name = medical.Name,
                             register = medical.register,
                             CreateBy = medical.CreateBy,
                             CreationDate = medical.CreationDate,
                             isDeleted = medical.isDeleted,
                             patologyId = patology.Id,
                             patologyName = patology.Name,
                             UpdateBy = medical.UpdateBy,
                             UpdateDate = medical.UpdateDate,
                         })).ToListAsync();
        }

        public async Task<IEnumerable<MedicalDto>> GetMedicalByPatolgy(int idPatology)
        {
            return await (from medical in _db.Medical
                          join patology in _db.Patology on medical.patology.Id equals patology.Id
                          where patology.Id == idPatology
                          select (new MedicalDto
                          {
                              Id = medical.Id,
                              Name = medical.Name,
                              register = medical.register,
                              CreateBy = medical.CreateBy,
                              CreationDate = medical.CreationDate,
                              isDeleted = medical.isDeleted,
                              patologyId = patology.Id,
                              patologyName = patology.Name,
                              UpdateBy = medical.UpdateBy,
                              UpdateDate = medical.UpdateDate,
                          })).ToListAsync();
        }

        public async Task<IEnumerable<MedicalDto>> GetMedicals()
        {
            return await(from medical in _db.Medical
                         join patology in _db.Patology on medical.patology.Id equals patology.Id
                         select (new MedicalDto
                         {
                             Id = medical.Id,
                             Name = medical.Name,
                             register = medical.register,
                             CreateBy = medical.CreateBy,
                             CreationDate = medical.CreationDate,
                             isDeleted = medical.isDeleted,
                             patologyId = patology.Id,
                             patologyName = patology.Name,
                             UpdateBy = medical.UpdateBy,
                             UpdateDate = medical.UpdateDate,
                         })).ToListAsync();
        }
    }
}
