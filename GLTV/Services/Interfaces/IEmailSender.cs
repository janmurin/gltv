﻿using System.Threading.Tasks;

namespace GLTV.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string author, EmailType type, object data);
    }
}
