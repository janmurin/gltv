using System.Collections.Generic;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, EmailType type, object data);
        void SendNewInzeratyNotifications(string userEmail, List<Inzerat> userInzerats, FilterData filterFilterData);
    }
}
