using AutoMapper;
using Luveck.Service.Security.Data;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Models.Dtos;
using Luveck.Service.Security.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        public AppDbContext _db;
        public IMapper _mapper;
        public ModuleRepository(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> delete(string name)
        {
            try
            {
                Module module = await _db.Modules.FirstOrDefaultAsync(c => c.name.ToLower() == name);

                if (module == null) return false;

                _db.Modules.Remove(module);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ModuleDto>> GetModules()
        {
            var modules = await _db.Modules.ToListAsync();
            return _mapper.Map<List<ModuleDto>>(modules);
        }

        public async Task<Module> Insert(string name)
        {            
            if (!(await _db.Modules.ToListAsync()).Exists(x=> x.name.ToLower() == name.ToLower()))
            {
                var module = new Module {
                    name = name
                };
                _db.Modules.Add(module);
            }
            await _db.SaveChangesAsync();

            return await _db.Modules.FirstOrDefaultAsync(x=> x.name.ToLower() == name.ToLower());
        }
    }
}
