using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GLTV.Data;
using GLTV.Models;
using GLTV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.FileProviders;

namespace GLTV.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{id?}")]
    public class TvItemsController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITvItemService _tvItemService;

        public TvItemsController(IFileService fileService, ITvItemService tvItemService)
        {
            _fileService = fileService;
            _tvItemService = tvItemService;
        }

        // GET: TvItems
        public async Task<IActionResult> Index()
        {
            List<TvItem> tvItems = _tvItemService.FetchTvItems();

            return View(tvItems);
        }

        // GET: TvItems/Details/5
        public async Task<IActionResult> Details(int id)
        {
            TvItem item = _tvItemService.FetchTvItem(id);

            return View(item);
        }

        // GET: TvItems/Create
        public IActionResult Create()
        {
            return View("Create", new TvItemCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> Create([Bind]TvItemCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // add item
                TvItem item = model.TvItem;
                item.Author = User.Identity.Name;
                item.TimeInserted = DateTime.Now;

                // add locations
                model.TvItem.Locations = new List<TvItemLocation>();
                if (model.LocationCheckboxes.LocationBanskaBystrica)
                {
                    model.TvItem.Locations.Add(new TvItemLocation() { Location = Location.BanskaBystrica });
                }

                if (model.LocationCheckboxes.LocationKosice)
                {
                    model.TvItem.Locations.Add(new TvItemLocation() { Location = Location.Kosice });
                }

                if (model.LocationCheckboxes.LocationZilina)
                {
                    model.TvItem.Locations.Add(new TvItemLocation() { Location = Location.Zilina });
                }

                _tvItemService.AddTvItem(item, model.TvItem.Locations);

                // add files
                List<TvItemFile> files = await _fileService.SaveFiles(item.ID, model.Files);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            TvItem item = _tvItemService.FetchTvItem(id);

            var model = new TvItemEditViewModel();
            model.TvItem = item;
            model.LocationCheckboxes.LocationBanskaBystrica =
                item.Locations.Any(x => x.Location == Location.BanskaBystrica);
            model.LocationCheckboxes.LocationKosice =
                item.Locations.Any(x => x.Location == Location.Kosice);
            model.LocationCheckboxes.LocationZilina =
                item.Locations.Any(x => x.Location == Location.Zilina);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind]TvItemEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                // update item
                TvItem item = _tvItemService.FetchTvItem(model.TvItem.ID);
                item.Title = model.TvItem.Title;
                item.StartTime = model.TvItem.StartTime;
                item.EndTime = model.TvItem.EndTime;
                item.Duration = model.TvItem.Duration;

                // update locations
                model.TvItem.Locations = new List<TvItemLocation>();
                if (model.LocationCheckboxes.LocationBanskaBystrica)
                {
                    model.TvItem.Locations.Add(new TvItemLocation() { TvItemId = item.ID, Location = Location.BanskaBystrica });
                }

                if (model.LocationCheckboxes.LocationKosice)
                {
                    model.TvItem.Locations.Add(new TvItemLocation() { TvItemId = item.ID, Location = Location.Kosice });
                }

                if (model.LocationCheckboxes.LocationZilina)
                {
                    model.TvItem.Locations.Add(new TvItemLocation() { TvItemId = item.ID, Location = Location.Zilina });
                }

                _tvItemService.UpdateTvItem(item, model.TvItem.Locations);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            TvItem item = _tvItemService.FetchTvItem(id);

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _tvItemService.DeleteTvItem(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
