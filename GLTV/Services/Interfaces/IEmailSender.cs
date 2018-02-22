using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string senderEmail, EmailType type, object data);
    }
}
