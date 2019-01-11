using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
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
                TvItemFile itemFile = await tvItemService.FetchTvItemFileAsync(filename);

                //string headers = "";
                //foreach (var key in context.Request.Headers.Keys)
                //    headers += key + "=" + context.Request.Headers[key] + Environment.NewLine;
                //Console.WriteLine(headers);

                if (itemFile != null)
                {
                    await eventService.AddClientEventAsync(
                        //context.Connection.RemoteIpAddress.ToString(),
                        context.Request.Headers["X-Forwarded-For"],
                        itemFile.IsVideoFile() ? ClientEventType.VideoRequest : ClientEventType.ImageRequest,
                        "",
                        itemFile.ID);
                }
                else
                {
                    await eventService.AddClientEventAsync(
                        //context.Connection.RemoteIpAddress.ToString(),
                        context.Request.Headers["X-Forwarded-For"],
                        ClientEventType.Exception,
                        $"File [{filename}] not found.",
                        null);
                }
            }

            if (requestPath.Contains("/api/read/"))
            {
                // fix incorrect request path if necessary
                if (requestPath.StartsWith("//"))
                {
                    context.Request.Path = requestPath.Substring(1);
                }
            }

            await _next(context);
        }
    }
}
