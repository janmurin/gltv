using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;

namespace GLTV.Services.Interfaces
{
    public interface IEventService
    {
        Task LogEventAsync(string author, LogEventType type, string message, int? itemId);
        List<LogEvent> FetchLogEventsAsync();
        Task ClientEventAsync(string source, ClientEventType type, string message, int? itemId);
        List<ClientEvent> FetchClientEventsAsync();
    }
}
