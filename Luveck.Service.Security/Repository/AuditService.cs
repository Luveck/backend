using Luveck.Service.Security.DTO;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.UnitWork;
using System;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> RegisterAudit(AuditRequestDto audit, string user)
        {
            try
            {
                await _unitOfWork.AuditRepository.InsertAsync(new Audit()
                {
                    Id = string.Concat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute,
                    DateTime.Now.Second, DateTime.Now.Millisecond),
                    Device = audit.Device,
                    IP = audit.IP,
                    LoginDate = DateTime.Now,
                    userId = user
                }) ;
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
