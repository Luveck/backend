using System.Threading.Tasks;

namespace Luveck.Service.Security.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
