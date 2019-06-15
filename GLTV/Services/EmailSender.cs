using System;
using System.Net.Mail;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Diagnostics;

namespace GLTV.Services
{
    public enum EmailType
    {
        Insert = 0,
        Error = 1
    }

    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string author, EmailType type, object data)
        {
            try
            {
                
                if (type == EmailType.Error)
                {
                    IExceptionHandlerFeature o = (IExceptionHandlerFeature)data;

                    MailMessage mailMessage = new MailMessage();
                    MailAddress fromAddress = new MailAddress("server-error@scraper.sk");
                    mailMessage.To.Add(Constants.SERVER_ADMIN);
                    mailMessage.From = fromAddress;
                    mailMessage.Body = string.Format("Server error occured on request by [{0}]\n\n" +
                                                     "Message: {1}\n" +
                                                     "StackTrace: {2}\n" +
                                                     "\n" +
                                                     "This is automated message from gltv server. In case of need, contact server administrator: {3}",
                        author, o.Error.Message, o.Error.StackTrace, Constants.SERVER_ADMIN);
                    mailMessage.IsBodyHtml = false;
                    mailMessage.Subject = "scraper.sk ERROR";
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "localhost";
                    smtpClient.Send(mailMessage);

                    // send one more message to The Architect of this server
                    mailMessage.To.Clear();
                    mailMessage.To.Add(Constants.THE_ARCHITECT);
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
