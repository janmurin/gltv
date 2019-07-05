using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.Objects
{
    public class UserSetting
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public bool NotificationsEnabled { get; set; }
    }
}
