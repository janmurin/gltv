using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GLTV.Services
{
    public enum EmailType
    {
        Insert = 0,
        Error = 1,
        Notification = 2
    }

    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string recipientEmail, EmailType type, object data)
        {
            try
            {

                if (type == EmailType.Error)
                {
                    IExceptionHandlerFeature o = (IExceptionHandlerFeature)data;

                    MailMessage mailMessage = new MailMessage();
                    MailAddress fromAddress = new MailAddress("server-error@scraper.sk");
                    mailMessage.To.Add(recipientEmail);
                    mailMessage.From = fromAddress;
                    mailMessage.Body = string.Format("Server error occured on request by [{0}]\n\n" +
                                                     "Message: {1}\n" +
                                                     "StackTrace: {2}\n" +
                                                     "\n" +
                                                     "This is automated message from gltv server. In case of need, contact server administrator: {3}",
                        recipientEmail, o.Error.Message, o.Error.StackTrace, Constants.SERVER_ADMIN);
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

                if (type == EmailType.Notification)
                {
                    string o = (string)data;
                    Console.WriteLine($"sending notification email to {recipientEmail}. Message is '{o}'");

                    MailMessage mailMessage = new MailMessage();
                    MailAddress fromAddress = new MailAddress("no-reply@scraper.sk");
                    mailMessage.From = fromAddress;
                    mailMessage.To.Add(recipientEmail);
                    mailMessage.Body = o;
                    //mailMessage.Body = string.Format("New content was inserted by [{0}]\n\n" +
                    //                                 "Title: {1}\n" +
                    //                                 "Type: {2}\n" +
                    //                                 "StartTime: {3}\n" +
                    //                                 "EndTime: {4}\n" +
                    //                                 "Duration: {5}\n" +
                    //                                 "Locations: {6}\n" +
                    //                                 "\n" +
                    //                                 "Url: {7}\n\n" +
                    //                                 "This is automated message from gltv server. In case of need, contact server administrator: {8}",
                    //    o.Author, o.Title, o.Type.ToString(), o.StartTime.ToString("dd.MM.yyyy HH:mm"), o.EndTime.ToString("dd.MM.yyyy HH:mm"),
                    //    Utils.GetFormattedDuration(o),
                    //    Utils.GetLocationsString(o.Locations), o.GetAnonymousDetailUrl, Constants.SERVER_ADMIN);
                    mailMessage.IsBodyHtml = false;
                    mailMessage.Subject = "Test info message";
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "localhost";
                    smtpClient.Send(mailMessage);

                    //// send notification to admins as well
                    //if (!Constants.SERVER_ADMIN.Contains(o.Author))
                    //{
                    //    mailMessage.To.Clear();
                    //    mailMessage.To.Add(Constants.SERVER_ADMIN);
                    //    smtpClient.Send(mailMessage);
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }

        public void SendNewInzeratyNotifications(string userEmail, List<Inzerat> userInzerats,
            FilterData filterFilterData)
        {
            Console.WriteLine($"sending notification email to {userEmail}. Inzeraty size is '{userInzerats.Count}'");

            MailMessage mailMessage = new MailMessage();
            MailAddress fromAddress = new MailAddress("no-reply@scraper.sk");
            mailMessage.From = fromAddress;
            mailMessage.To.Add(userEmail);
            StringBuilder sb = new StringBuilder($"<h3>Bolo nájdených {userInzerats.Count} inzerátov, ktoré vyhovujú vašemu zvolenému filtru:</h3><ul>");
            foreach (Inzerat inzerat in userInzerats.Take(10))
            {
                sb.Append(
                    $"<li><a target=\"_blank\" href=\"{inzerat.Url}\">{inzerat.Title}</a></li>");
            }
            sb.Append($"</ul><h4>Parametre Vášho filtra:</h4> " +
                      $"<dl> " +
                      $"<dt>Typ</dt> <dd>{filterFilterData.InzeratType}</dd> " +
                      $"<dt>Kategória</dt> <dd>{filterFilterData.InzeratCategory}</dd> " +
                      $"<dt>Lokácia</dt> <dd>{filterFilterData.Location}</dd> " +
                      $"<dt>Maximálna cena</dt> <dd>{filterFilterData.PriceString} €</dd> " +
                      $"</dl> " +
                      $"<br> " +
                      $"<strong>Ak si neželáte dostávať email o najnovších inzerátoch, zmeňte svoje <a target=\"_blank\" href=\"http://scraper.sk/home/settings\">nastavenia</a>.</strong>");
            mailMessage.Body = sb.ToString();
            mailMessage.IsBodyHtml = false;
            mailMessage.Subject = "scraper.sk: Nové inzeráty";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "localhost";
            smtpClient.Send(mailMessage);

        }
    }
}
