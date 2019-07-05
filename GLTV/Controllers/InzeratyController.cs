using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;
using GLTV.Models.ViewModels;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Controllers
{
    [Authorize]
    public class InzeratyController : Controller
    {
        private readonly IInzeratyService _inzeratyService;
        private readonly IUserService _userService;

        public InzeratyController(IInzeratyService inzeratyService, IUserService userService)
        {
            _inzeratyService = inzeratyService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(string inzeratType, string inzeratCategory, string location, string priceString, int? pageNumber)
        {
            FilterData filterData = new FilterData();
            int priceMax = 0;

            if (string.IsNullOrEmpty(inzeratCategory) && string.IsNullOrEmpty(inzeratType) &&
                string.IsNullOrEmpty(location) && string.IsNullOrEmpty(priceString))
            {
                // if no filter, load user's filter data
                filterData = await _userService.FetchUserFilterDataAsync();
                Int32.TryParse(filterData.PriceString, out priceMax);
            }
            else
            {
                // parse filter data from request and save in DB
                filterData.InzeratCategory = inzeratCategory;
                filterData.InzeratType = inzeratType;
                filterData.Location = location;
                Int32.TryParse(priceString, out priceMax);
                filterData.PriceString = priceMax.ToString();

                await _userService.UpdateUserFilterDataAsync(filterData);
            }

            int pageSize = 100;
            var model = new InzeratyViewModel();


            model.Inzeraty = await _inzeratyService.FetchInzeratyAsync(
                filterData.InzeratType,
                filterData.InzeratCategory,
                filterData.Location,
                priceMax,
                pageNumber ?? 1,
                pageSize);

            model.InzeratyTypes = new SelectList(await _inzeratyService.FetchInzeratyTypesAsync());
            model.InzeratyCategories = new SelectList(await _inzeratyService.FetchInzeratyCategoriesAsync());
            model.Locations = new SelectList(await _inzeratyService.FetchInzeratyLocationsAsync());
            model.InzeratType = filterData.InzeratType;
            model.InzeratCategory = filterData.InzeratCategory;
            model.Location = filterData.Location;
            model.PriceString = filterData.PriceString;

            return View(model);
        }

        public async Task<IActionResult> Marked(string inzeratType, string inzeratCategory, string location, string priceString, int? pageNumber)
        {
            FilterData filterData = new FilterData();
            int priceMax = 0;

            // parse filter data from request and save in DB
            filterData.InzeratCategory = inzeratCategory;
            filterData.InzeratType = inzeratType;
            filterData.Location = location;
            Int32.TryParse(priceString, out priceMax);
            filterData.PriceString = priceMax.ToString();

            int pageSize = 100;
            var model = new InzeratyViewModel();
            
            model.Inzeraty = await _inzeratyService.FetchMarkedInzeratyAsync(
                filterData.InzeratType,
                filterData.InzeratCategory,
                filterData.Location,
                priceMax,
                pageNumber ?? 1,
                pageSize);

            model.InzeratyTypes = new SelectList(await _inzeratyService.FetchInzeratyTypesAsync());
            model.InzeratyCategories = new SelectList(await _inzeratyService.FetchInzeratyCategoriesAsync());
            model.Locations = new SelectList(await _inzeratyService.FetchInzeratyLocationsAsync());
            model.InzeratType = filterData.InzeratType;
            model.InzeratCategory = filterData.InzeratCategory;
            model.Location = filterData.Location;
            model.PriceString = filterData.PriceString;

            return View(model);
        }

        public async Task<IActionResult> Ignored(string inzeratType, string inzeratCategory, string location, string priceString, int? pageNumber)
        {
            FilterData filterData = new FilterData();
            int priceMax = 0;

            // parse filter data from request and save in DB
            filterData.InzeratCategory = inzeratCategory;
            filterData.InzeratType = inzeratType;
            filterData.Location = location;
            Int32.TryParse(priceString, out priceMax);
            filterData.PriceString = priceMax.ToString();

            int pageSize = 100;
            var model = new InzeratyViewModel();

            model.Inzeraty = await _inzeratyService.FetchIgnoredInzeratyAsync(
                filterData.InzeratType,
                filterData.InzeratCategory,
                filterData.Location,
                priceMax,
                pageNumber ?? 1,
                pageSize);

            model.InzeratyTypes = new SelectList(await _inzeratyService.FetchInzeratyTypesAsync());
            model.InzeratyCategories = new SelectList(await _inzeratyService.FetchInzeratyCategoriesAsync());
            model.Locations = new SelectList(await _inzeratyService.FetchInzeratyLocationsAsync());
            model.InzeratType = filterData.InzeratType;
            model.InzeratCategory = filterData.InzeratCategory;
            model.Location = filterData.Location;
            model.PriceString = filterData.PriceString;

            return View(model);
        }
    }
}