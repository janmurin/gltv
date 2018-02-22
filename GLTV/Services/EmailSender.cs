using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
//using System.Net.Mail;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace GLTV.Services
{
    public enum EmailType
    {
        Insert = 0
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailConfig> emailConfig)
        {
            this.Ec = emailConfig.Value;
        }

        public EmailConfig Ec { get; set; }

        public Task SendEmailAsync(string author, EmailType type, object data)
        {
            try
            {
                if (type == EmailType.Insert)
                {
                    TvItem o = (TvItem)data;

                    MailMessage mailMessage = new MailMessage();
                    MailAddress fromAddress = new MailAddress("gltv-server@gltvslovakia.globallogic.com");
                    mailMessage.From = fromAddress;
                    mailMessage.To.Add(author + "@globallogic.com");
                    mailMessage.Body = string.Format("New content was inserted by [{0}]\n\n" +
                                                     "Title: {1}\n" +
                                                     "Type: {2}\n" +
                                                     "StartTime: {3}\n" +
                                                     "EndTime: {4}\n" +
                                                     "Duration: {5}\n" +
                                                     "Locations: {6}\n" +
                                                     "\n" +
                                                     "Url: {7}\n\n" +
                                                     "This is automated message from gltv server. In case of need, contact server administrator: {8}",
                        o.Author, o.Title, o.Type.ToString(), o.StartTime.ToString("dd.MM.yyyy HH:mm"), o.EndTime.ToString("dd.MM.yyyy HH:mm"),
                        Utils.GetFormattedDuration(o),
                        Utils.GetLocationsString(o.Locations), Constants.SERVER_URL + "/TvItems/DetailsAnonymous/" + o.ID, Ec.ServerAdmins.FirstOrDefault());
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = "GLTV insert";
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "localhost";
                    smtpClient.Send(mailMessage);

                    // send notification to admins as well
                    foreach (string admin in Ec.ServerAdmins)
                    {
                        if (!admin.Contains(o.Author))
                        {
                            mailMessage.To.Clear();
                            mailMessage.To.Add(admin);
                            smtpClient.Send(mailMessage);
                        }
                    }
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
