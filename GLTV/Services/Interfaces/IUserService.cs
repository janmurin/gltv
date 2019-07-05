using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Services.Interfaces
{
    public interface IUserService
    {
        Task<FilterData> FetchUserFilterDataAsync();
        Task<FilterData> UpdateUserFilterDataAsync(FilterData filterData);
        Task<UserSetting> FetchUserSettingAsync();
        Task<UserSetting> UpdateUserSettingAsync(UserSetting userSetting);
    }
}
