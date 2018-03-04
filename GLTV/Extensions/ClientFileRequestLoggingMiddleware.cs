using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Services;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
//using Org.BouncyCastle.Asn1.Ocsp;

namespace GLTV.Extensions
{
    public class ClientFileRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientFileRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db, IEventService eventService, ITvItemService tvItemService)
        {
            string requestPath = context.Request.Path.ToString();

            if (requestPath.EndsWith(Constants.CLIENT_FILE_REQUEST_SUFFIX))
            {
                //Console.WriteLine("file request url: " + requestPath);
                string truncatedPath = requestPath.Substring(0, requestPath.Length - Constants.CLIENT_FILE_REQUEST_SUFFIX.Length);
                context.Request.Path = truncatedPath;
                //Console.WriteLine("file request url changed to: " + context.Request.Path.ToString());

                string filename = truncatedPath.Substring(requestPath.LastIndexOf('/') + 1);
                TvItemFile itemFile = tvItemService.GetTvItemFile(filename);

                if (itemFile != null)
                {
                    await eventService.AddClientEventAsync(
                        context.Connection.RemoteIpAddress.ToString(),
                        itemFile.IsVideoFile() ? ClientEventType.VideoRequest : ClientEventType.ImageRequest,
                        "",
                        itemFile.ID);
                }
                else
                {
                    await eventService.AddClientEventAsync(
                        context.Connection.RemoteIpAddress.ToString(),
                        ClientEventType.Exception,
                        $"File [{filename}] not found.",
                        null);
                }

            }

            await _next(context);
        }
    }
}
