using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GLTV.Services
{
    public class LogEventService : ServiceBase, ILogEventService
    {
        public LogEventService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {

        }

        public Task<List<ScraperLogEvent>> FetchScraperLogEventsAsync()
        {
            List<ScraperLogEvent> logEvents = Context.ScraperLogEvent.OrderByDescending(q => q.ID).ToList();
            return Task.FromResult(logEvents);
        }
    }
}
