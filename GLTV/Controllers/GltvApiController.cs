using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GLTV.Controllers
{
    [AllowAnonymous]
    public class GltvApiController : Controller
    {
        private readonly IInzeratyService _inzeratyService;
        private IEmailSender _emailSender;

        public GltvApiController(IInzeratyService inzeratyService, IEmailSender emailSender)
        {
            _inzeratyService = inzeratyService;
            _emailSender = emailSender;
        }

        [Authorize]
        [Produces("application/json")]
        [Route("/api/ignoreInzerat/{id:int}")]
        public async Task<IActionResult> IgnoreInzerat(int id)
        {
            ObjectResult objectResult = await _inzeratyService.IgnoreInzeratForUser(id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    return new ObjectResult(new { id = id, username = HttpContext.User.Identity.Name });
                }
                else
                {
                    return new ObjectResult(new { message = $"{task.Exception?.Message}" });
                }
            });

            return objectResult;
        }

        [Authorize]
        [Produces("application/json")]
        [Route("/api/markInzerat/{id:int}")]
        public async Task<IActionResult> MarkInzerat(int id)
        {
            ObjectResult objectResult = await _inzeratyService.MarkInzeratForUser(id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    return new ObjectResult(new { id = id, username = HttpContext.User.Identity.Name });
                }
                else
                {
                    return new ObjectResult(new { message = $"{task.Exception?.Message}" });
                }
            });

            return objectResult;
        }

        [Authorize]
        [Produces("application/json")]
        [Route("/api/cancelIgnoredInzerat/{id:int}")]
        public async Task<IActionResult> CancelIgnoredInzerat(int id)
        {
            ObjectResult objectResult = await _inzeratyService.CancelIgnoredInzeratForUser(id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    return new ObjectResult(new { id = id, username = HttpContext.User.Identity.Name });
                }
                else
                {
                    return new ObjectResult(new { message = $"{task.Exception?.Message}" });
                }
            });

            return objectResult;
        }

        [Authorize]
        [Produces("application/json")]
        [Route("/api/cancelMarkedInzerat/{id:int}")]
        public async Task<IActionResult> CancelMarkedInzerat(int id)
        {
            ObjectResult objectResult = await _inzeratyService.CancelMarkedInzeratForUser(id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    return new ObjectResult(new { id = id, username = HttpContext.User.Identity.Name });
                }
                else
                {
                    return new ObjectResult(new { message = $"{task.Exception?.Message}" });
                }
            });

            return objectResult;
        }

        [Authorize]
        [Produces("application/json")]
        [Route("/api/sendNotificationEmail")]
        public async Task<IActionResult> SendNotificationEmail()
        {
            string userName = HttpContext.User.Identity.Name;
            string message = "test notification message from api";

            ObjectResult objectResult = await _emailSender.SendEmailAsync($"{userName}@gmail.com", EmailType.Notification, message)
                .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    return new ObjectResult(new { username = userName, message = message });
                }
                else
                {
                    return new ObjectResult(new { message = $"{task.Exception?.Message}" });
                }
            });

            return objectResult;
        }
    }
}