using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Services.Interfaces
{
    public interface ILogEventService
    {
        Task<List<ScraperLogEvent>> FetchScraperLogEventsAsync();
    }
}
