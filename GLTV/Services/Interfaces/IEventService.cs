﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GLTV.Models;
using GLTV.Models.Objects;

namespace GLTV.Services.Interfaces
{
    public interface IEventService
    {
        Task AddWebServerLogAsync(string author, WebServerLogType type, string message, int? itemId);
        Task<List<WebServerLog>> FetchWebServerActivitiesAsync();
        Task AddFileRequestEventAsync(string sourceIp, string filename);
        Task AddHandShakeAsync(string sourceIp, WebClientLogType type, Location location);
        Task<List<TvScreen>> FetchActiveScreensAsync();

        Task MigrateData();
    }
}
