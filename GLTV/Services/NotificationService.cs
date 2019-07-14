using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace GLTV.Services
{
    public class NotificationService : INotificationService
    {
        private ApplicationDbContext Context;
        private IInzeratyService _inzeratyService;
        private IUserService _userService;
        private IEmailSender _emailSender;
        private UserManager<ApplicationUser> _userManager;

        public NotificationService(
            ApplicationDbContext context, 
            IInzeratyService inzeratyService, 
            IUserService userService, 
            IEmailSender emailSender, 
            UserManager<ApplicationUser> userManager)
        {
            Context = context;
            _inzeratyService = inzeratyService;
            _userService = userService;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        /// <summary>
        /// this method should not be called more often than each 10 minutes
        ///
        /// 
        /// </summary>
        /// <returns></returns>
        public Task SendNewInzeratyNotifications()
        {
            Console.WriteLine("sending notifications");
            // get list of users with their search filters, that want notifications sent through email
            List<UserFilter> userFilters = FetchUserFilterDataForNotifications();
             
            // for each user get new inzeraty and send notification to him
            Dictionary<string, List<Inzerat>> dictionary = new Dictionary<string, List<Inzerat>>();
            foreach (UserFilter filter in userFilters)
            {
                ApplicationUser user = _userManager.Users.FirstOrDefault(u => u.NormalizedUserName.Equals(filter.Username, StringComparison.InvariantCultureIgnoreCase));
                if (user is null)
                {
                    Console.WriteLine($"SendNewInzeratyNotifications: USER {filter.Username} NOT FOUND. not sending email notification");
                    continue;
                }

                List<Inzerat> userInzerats = _inzeratyService.FetchInzeratyForNotifications(filter.FilterData);
                
                dictionary.Add(filter.Username, userInzerats);
                if (userInzerats.Count > 0)
                {
                    _emailSender.SendNewInzeratyNotifications(user.Email, userInzerats, filter.FilterData);
                }
            }

            // create notification log
            Inzerat first = Context.Inzerat.OrderByDescending(i => i.ID).First();
            NotificationLog nl = new NotificationLog()
            {
                InzeratID = first.ID,
                TimeInserted = DateTime.Now,
                Message = GetNotifiedUsersJson(dictionary)
            };

            Context.Update(nl);
            Context.SaveChanges();


            return Task.CompletedTask;
        }

        private string GetNotifiedUsersJson(Dictionary<string, List<Inzerat>> userInzeraty)
        {
            List<NotificationMessage> users = new List<NotificationMessage>();
            foreach (KeyValuePair<string, List<Inzerat>> keyValuePair in userInzeraty)
            {
                users.Add(new NotificationMessage()
                {
                    Count = keyValuePair.Value.Count,
                    User = keyValuePair.Key
                });
            }

            return JsonConvert.SerializeObject(users);

        }

        private List<UserFilter> FetchUserFilterDataForNotifications()
        {
            List<UserFilter> userFilters = (from filter in Context.UserFilter
                join setting in Context.UserSetting
                    on filter.Username equals setting.Username
                where setting.NotificationsEnabled
                select filter).ToList();
            foreach (UserFilter filter in userFilters)
            {
                filter.FilterData = JsonConvert.DeserializeObject<FilterData>(filter.FilterDataJson);
            }

            return userFilters;
        }
    }
}
