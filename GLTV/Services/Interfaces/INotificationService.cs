using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNewInzeratyNotifications();
    }
}
