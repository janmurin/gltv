using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;

namespace GLTV.Services.Interfaces
{
    public interface IEventService
    {
        Task AddLogEventAsync(string author, LogEventType type, string message, int? itemId);
        List<LogEvent> FetchLogEventsAsync();
        Task AddClientEventAsync(string source, ClientEventType type, string message, int? itemId);
        List<ClientEvent> FetchClientEventsAsync();
    }
}
