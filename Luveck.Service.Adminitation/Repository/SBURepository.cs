using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository
{
    public class SBURepository : ISBURepository
    {
        public readonly AppDbContext _db;
        public IMapper _mapper;

        public SBURepository( AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        //public async Task<SBUDto> CreateUpdateSBU(SBUDto Sbu)
        //{
        //    SBU sbu = _mapper.Map<SBU>(Sbu);

        //    if (sbu.Id > 0)
        //    {
        //        _db.SBU.Update(sbu);
        //    }
        //    else
        //    {
        //        _db.SBU.Add(sbu);
        //    }

        //    await _db.SaveChangesAsync();

        //    return _mapper.Map<SBUDto>(sbu);
        //}

        //public async Task<SBUDto> GetSBU(int id)
        //{
        //    SBU Sbu = await _db.SBU.FirstOrDefaultAsync(c => c.Id == id);
        //    return _mapper.Map<SBUDto>(Sbu);            
        //}

        //public async Task<IEnumerable<SBUDto>> GetSBUs()
        //{
        //    List<SBU> sbuList = await _db.SBU.ToListAsync();
        //    return _mapper.Map<List<SBUDto>>(sbuList);            
        //}
    }
}
