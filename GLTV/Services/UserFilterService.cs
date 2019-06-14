﻿using System;
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
    public class UserFilterService : ServiceBase, IUserFilterService
    {

        public UserFilterService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
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
    }
}
