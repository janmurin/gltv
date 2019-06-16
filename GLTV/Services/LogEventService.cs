using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;

namespace GLTV.Services
{
    public class LogEventService : ServiceBase, ILogEventService
    {
        public LogEventService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {

        }

        public Task<PaginatedList<ScraperLogEvent>> FetchScraperLogEventsAsync(int pageNumber)
        {
            int topEventID = Context.ScraperLogEvent.OrderByDescending(x => x.ID).First().EventID;
            // one page contains 10 events
            int pageSize = 10;
            int topId = topEventID - (pageNumber - 1) * pageSize;
            int bottomId = Math.Max(0, topEventID - (pageNumber + 1) * pageSize);

            List<ScraperLogEvent> events = Context.ScraperLogEvent
                .Where(x => x.EventID <= topId && x.EventID > bottomId)
                .OrderByDescending(x => x.ID)
                .ToList();

            return Task.FromResult(PaginatedList<ScraperLogEvent>.CreateForLogEvents(events, topEventID, pageNumber, pageSize));
        }
    }
}
