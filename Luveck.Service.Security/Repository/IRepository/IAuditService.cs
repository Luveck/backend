using Luveck.Service.Security.DTO;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IAuditService
    {
        Task<bool> RegisterAudit(AuditRequestDto audit, string user);
    }
}
