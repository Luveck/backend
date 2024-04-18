using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.UnitWork;
using Luveck.Service.Security.Utils.Exceptions;
using Luveck.Service.Security.Utils.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModuleRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> delete(string name)
        {
            var exist = await _unitOfWork.ModuleRepository.FirstOrDefaultNoTracking(x => x.name.ToLower() == name.ToLower());
            if (exist == null) throw new BusinessException(GeneralMessage.ModuleExist);
            try
            {
                _unitOfWork.ModuleRepository.Delete(exist);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ModuleResponseDto>> GetModules()
        {
            List<ModuleResponseDto> lst = _unitOfWork.ModuleRepository.AsQueryable().Select(x => new ModuleResponseDto()
            {
                Id= x.Id,
                name = x.name,
            }).ToList();

            return lst;
        }

        public async Task<GeneralResponseDto> Insert(string name)
        {            
            var exist = await _unitOfWork.ModuleRepository.FirstOrDefaultNoTracking(x=> x.name.ToLower()==name.ToLower());
            if (exist != null) throw new BusinessException(GeneralMessage.ModuleExist);
            try
            {
                await _unitOfWork.ModuleRepository.InsertAsync(new Module() { name = name });
                return new GeneralResponseDto() { Code = "201", Message = GeneralMessage.ModuleCreate };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
