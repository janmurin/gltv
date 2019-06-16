using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models.Objects;

namespace GLTV.Models.ViewModels
{
    public class LogEventViewModel
    {
        public PaginatedList<ScraperLogEvent> LogEvents { get; set; }
    }
}
