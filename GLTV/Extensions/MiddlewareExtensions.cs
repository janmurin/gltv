using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace GLTV.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseConventionalMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClientFileRequestLoggingMiddleware>();
        }
    }
}
