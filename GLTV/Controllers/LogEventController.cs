using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.ViewModels;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GLTV.Controllers
{
    [Authorize]
    public class LogEventController : Controller
    {
        private readonly ILogEventService _logEventService;

        public LogEventController(ILogEventService logEventService)
        {
            _logEventService = logEventService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            LogEventViewModel model = new LogEventViewModel();
            model.LogEvents = await _logEventService.FetchScraperLogEventsAsync(pageNumber ?? 1);

            return View(model);
        }
    }
}