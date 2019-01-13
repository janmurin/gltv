using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GLTV.Extensions
{
    public class ClientFileRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientFileRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IEventService eventService)
        {
            string requestPath = context.Request.Path.ToString();

            if (requestPath.EndsWith(Constants.CLIENT_FILE_REQUEST_SUFFIX))
            {
                //Console.WriteLine("file request url: " + requestPath);
                string truncatedPath = requestPath.Substring(0, requestPath.Length - Constants.CLIENT_FILE_REQUEST_SUFFIX.Length);
                context.Request.Path = truncatedPath;
                //Console.WriteLine("file request url changed to: " + context.Request.Path.ToString());

                string filename = truncatedPath.Substring(requestPath.LastIndexOf('/') + 1);
                
                await eventService.AddFileRequestEventAsync(context.Request.Headers["X-Forwarded-For"], filename);
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
