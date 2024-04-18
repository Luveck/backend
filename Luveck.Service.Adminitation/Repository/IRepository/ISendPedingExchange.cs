using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface ISendPedingExchange
    {
        Task<bool> sendMasiveMailRemainder();
    }
}
