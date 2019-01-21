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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GLTV.Controllers
{
    [AllowAnonymous]
    public class GltvApiController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITvItemService _tvItemService;
        private readonly IEventService _eventService;

        public GltvApiController(IFileService fileService, ITvItemService tvItemService, IEventService eventService)
        {
            _fileService = fileService;
            _tvItemService = tvItemService;
            _eventService = eventService;
        }

        [Produces("application/json")]
        [Route("/api/read/maincontent/location/{locationID:int}/{token}")]
        public async Task<IActionResult> GetById(int locationID, string token)
        {
            if (!string.Equals(token, Constants.ANDROID_TOKEN))
            {
                throw new AuthenticationException("Bad token used.");
            }

            Location location = (Location)locationID;
            if (!Enum.IsDefined(typeof(Location), location) && !location.ToString().Contains(","))
                throw new InvalidOperationException($"{locationID} is not an underlying value of the Location enumeration.");

            List<TvItem> items = await _tvItemService.FetchActiveTvItemsAsync(location);

            await _eventService.AddHandShakeAsync(HttpContext.Connection.RemoteIpAddress.ToString(), WebClientLogType.ProgramRequest, location);

            return new ObjectResult(items.Select(x => new MainContentResponse(x)).ToList());
        }

        [Produces("application/json")]
        [Route("/api/read/chatmessages/location/{locationID:int}/{token}")]
        public async Task<IActionResult> GetChatMessages(int locationID, string token)
        {
            if (!string.Equals(token, Constants.ANDROID_TOKEN))
            {
                throw new AuthenticationException("Bad token used.");
            }

            Location location = (Location)locationID;
            if (!Enum.IsDefined(typeof(Location), location) && !location.ToString().Contains(","))
                throw new InvalidOperationException($"{locationID} is not an underlying value of the Location enumeration.");

            await _eventService.AddHandShakeAsync(HttpContext.Connection.RemoteIpAddress.ToString(), WebClientLogType.ChatRequest, location);

            return new ObjectResult(new List<MainContentResponse>());
        }

        [Produces("application/json")]
        [Route("/api/migratedata")]
        public async Task<IActionResult> MigrateData()
        {
            ObjectResult objectResult = await _eventService.MigrateData().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    return new ObjectResult("finished");
                }
                else
                {
                    return new ObjectResult("error: " + task.Exception?.Message);
                }
            });

            return objectResult;
        }
    }
}