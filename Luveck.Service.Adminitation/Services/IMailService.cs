using System.Threading.Tasks;

namespace Luveck.Service.Administration.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
