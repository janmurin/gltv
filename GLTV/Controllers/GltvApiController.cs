﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GLTV.Controllers
{
    [Produces("application/json")]
    [AllowAnonymous]
    public class GltvApiController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITvItemService _tvItemService;

        public GltvApiController(IFileService fileService, ITvItemService tvItemService)
        {
            _fileService = fileService;
            _tvItemService = tvItemService;
        }

        [Route("/api/read/maincontent/location/{locationID:int}/{token}")]
        public IActionResult GetById(int locationID, string token)
        {
            if (!string.Equals(token, Constants.ANDROID_TOKEN))
            {
                throw new AuthenticationException("Bad token used.");
            }

            Location location = (Location)locationID;
            if (!Enum.IsDefined(typeof(Location), location) && !location.ToString().Contains(","))
                throw new InvalidOperationException(
                    $"{locationID} is not an underlying value of the Location enumeration.");

            List<TvItem> items = _tvItemService.FetchActiveTvItems(location);

            return new ObjectResult(items.Select(x => new MainContentResponse(x)).ToList());
        }
    }
}