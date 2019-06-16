using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GLTV.Controllers
{
    [Authorize]
    public class LogEventController : Controller
    {
        private readonly IInzeratyService _inzeratyService;
        private readonly IUserFilterService _userFilterService;

        public LogEventController(IInzeratyService inzeratyService, IUserFilterService userFilterService)
        {
            _inzeratyService = inzeratyService;
            _userFilterService = userFilterService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}