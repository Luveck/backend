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

namespace Luveck.Service.Administration.Repository
{
    public class PatologyRepository : IPatologyRepository
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;

        public PatologyRepository(IMapper mapper, AppDbContext appDbContext)
        {
            _db = appDbContext;
            _mapper = mapper;
        }
        public async Task<PatologyDto> CreateUpdatePatology(PatologyDto patologyDto)
        {
            Patology patology = _mapper.Map<Patology>(patologyDto);
            patology.UpdateDate = DateTime.Now;
            patology.UpdateBy = patologyDto.UpdateBy;

            if (patology.Id > 0)
            {                                
                _db.Patology.Update(patology);
            }
            else
            {
                var p = _db.Patology.FirstOrDefaultAsync(x => x.Name == patologyDto.Name);
                if (p != null) return _mapper.Map<PatologyDto>(p);
                patology.CreationDate = DateTime.Now;
                patology.CreateBy = patology.UpdateBy;
                _db.Patology.Add(patology);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<PatologyDto>(patology);
        }

        public async Task<bool> deletePatology(int id, string user)
        {
            try
            {
                Patology patology = await _db.Patology.FirstOrDefaultAsync(c => c.Id == id);

                if (patology == null) return false;

                patology.IsDeleted = true;
                patology.UpdateDate = DateTime.Now;
                patology.UpdateBy = user;
                _db.Patology.Update(patology);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<PatologyDto>> GetPatologies()
        {
            return await(from Patology in _db.Patology
                         select (new PatologyDto
                         {
                             Id = Patology.Id,
                             Name = Patology.Name,
                             isDeleted = Patology.IsDeleted,
                             CreateBy = Patology.CreateBy,
                             CreationDate = Patology.CreationDate,
                             UpdateBy = Patology.UpdateBy,
                             UpdateDate = Patology.UpdateDate
                         })).ToListAsync();
        }

        public async Task<PatologyDto> GetPatology(int id)
        {
            Patology patology= await _db.Patology.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<PatologyDto>(patology);
        }
    }
}
