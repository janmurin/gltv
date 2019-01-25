using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;
using GLTV.Models.ViewModels;
using GLTV.Services;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GLTV.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{id?}")]
    public class ClientActivityController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITvItemService _tvItemService;
        private readonly IEmailSender _emailSender;
        private readonly IEventService _eventService;

        public ClientActivityController(IFileService fileService, ITvItemService tvItemService, IEmailSender emailSender, IEventService eventService)
        {
            _fileService = fileService;
            _tvItemService = tvItemService;
            _emailSender = emailSender;
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            List<TvScreen> activeTvScreens = await _eventService.FetchActiveScreensAsync();

            ClientEventsViewModel model = new ClientEventsViewModel();
            model.ClientEvents = new List<WebClientLog>(); // empty list so far
            model.ActiveTvScreens = activeTvScreens;

            return View(model);
        }
    }
}