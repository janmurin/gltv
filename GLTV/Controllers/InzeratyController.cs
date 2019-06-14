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

        public async Task<IActionResult> Index(string inzeratType, string inzeratCategory, string location, string priceString, int? pageNumber)
        {
            int pageSize = 100;
            var model = new InzeratyViewModel();
            int priceMax = 0;
            Int32.TryParse(priceString, out priceMax);

            model.Inzeraty = await _inzeratyService.FetchInzeratyAsync(inzeratType, inzeratCategory, location, priceMax, pageNumber ?? 1, pageSize);
            model.InzeratyTypes = new SelectList(await _inzeratyService.FetchInzeratyTypesAsync());
            model.InzeratyCategories = new SelectList(await _inzeratyService.FetchInzeratyCategoriesAsync());
            model.Locations = new SelectList(await _inzeratyService.FetchInzeratyLocationsAsync());
            model.InzeratType = inzeratType;
            model.InzeratCategory = inzeratCategory;
            model.Location = location;
            model.PriceString = priceString;

            return View(model);
        }
    }
}