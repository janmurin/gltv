using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using Newtonsoft.Json;

namespace GLTV.Services
{
    public class UserService : ServiceBase, IUserService
    {

        public UserService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {

        }

        private UserFilter FetchUserFilter()
        {
            var userFilter = Context.UserFilter
                .SingleOrDefault(m => m.Username.Equals(CurrentUser.Identity.Name));

            if (userFilter == null)
            {
                userFilter = new UserFilter()
                {
                    Username = CurrentUser.Identity.Name,
                    FilterDataJson = JsonConvert.SerializeObject(new FilterData()
                    {
                        InzeratType = "",
                        InzeratCategory = "",
                        Location = "",
                        PriceString = ""
                    })
                };
                Context.Update(userFilter);
                Context.SaveChanges();
            }

            userFilter.FilterData = JsonConvert.DeserializeObject<FilterData>(userFilter.FilterDataJson);

            return userFilter;
        }

        public Task<FilterData> FetchUserFilterDataAsync()
        {
            UserFilter userFilter = FetchUserFilter();
            return Task.FromResult(userFilter.FilterData);
        }

        public Task<FilterData> UpdateUserFilterDataAsync(FilterData filterData)
        {
            UserFilter userFilter = FetchUserFilter();
            userFilter.FilterDataJson = JsonConvert.SerializeObject(filterData);
            Context.Update(userFilter);
            Context.SaveChanges();

            return Task.FromResult(userFilter.FilterData);
        }

        private UserSetting FetchUserSetting()
        {
            var userSetting = Context.UserSetting
                .SingleOrDefault(m => m.Username.Equals(CurrentUser.Identity.Name));

            if (userSetting == null)
            {
                userSetting = new UserSetting()
                {
                    Username = CurrentUser.Identity.Name,
                    NotificationsEnabled = false
                };
                Context.Update(userSetting);
                Context.SaveChanges();
            }

            return userSetting;
        }

        public Task<UserSetting> FetchUserSettingAsync()
        {
            UserSetting userSetting = FetchUserSetting();
            return Task.FromResult(userSetting);
        }

        public Task<UserSetting> UpdateUserSettingAsync(UserSetting userSetting)
        {
            UserSetting setting = FetchUserSetting();
            Context.Update(setting);
            Context.SaveChanges();

            return Task.FromResult(setting);
        }

        public List<UserFilter> FetchUserFilterDataForNotifications()
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
