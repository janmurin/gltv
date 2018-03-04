using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
//using Org.BouncyCastle.Asn1.Ocsp;

namespace GLTV.Extensions
{
    public class ConventionalMiddleware
    {
        private readonly RequestDelegate _next;

        public ConventionalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db, IEventService eventService)
        {
            string requestPath = context.Request.Path.ToString();

            if (requestPath.EndsWith("clientRequest"))
            {
                Console.WriteLine("file request url: " + requestPath);
                context.Request.Path = requestPath.Replace("clientRequest", "");
                Console.WriteLine("file request url changed to: " + context.Request.Path.ToString());
            }

            await _next(context);
        }
    }
}
