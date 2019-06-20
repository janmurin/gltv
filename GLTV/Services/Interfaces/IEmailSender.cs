using System.Threading.Tasks;

namespace GLTV.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, EmailType type, object data);
    }
}
