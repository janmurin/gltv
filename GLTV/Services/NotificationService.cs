using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Services.Interfaces;

namespace GLTV.Services
{
    public class NotificationService : INotificationService
    {
        public NotificationService(ApplicationDbContext context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; set; }

        public Task SendNewInzeratyNotifications()
        {
            Console.WriteLine("sending notifications");

            return Task.CompletedTask;
        }
    }
}
