using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;
using GLTV.Models.ViewModels;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Controllers
{
    public class InzeratyController : Controller
    {
        private readonly IInzeratyService _inzeratyService;

        public InzeratyController(IInzeratyService inzeratyService)
        {
            _inzeratyService = inzeratyService;
        }

        public async Task<IActionResult> Index(string inzeratType, string location, string priceString)
        {
            var model = new InzeratyViewModel();
            int priceMax = 0;
            Int32.TryParse(priceString, out priceMax);

            model.Inzeraty = await _inzeratyService.FetchInzeratyAsync(inzeratType, location, priceMax);
            model.InzeratyTypes = new SelectList(await _inzeratyService.FetchInzeratyTypesAsync());
            model.Locations = new SelectList(await _inzeratyService.FetchInzeratyLocationsAsync());

            return View(model);
        }
    }
}