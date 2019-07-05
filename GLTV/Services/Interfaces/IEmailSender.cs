using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, EmailType type, object data);
        Task<NotificationLog> FetchNotificationLogAsync();
        Task<NotificationLog> UpdateNotificationLogAsync(NotificationLog notificationLog);
    }
}
