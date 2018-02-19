using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GLTV.Models;
using GLTV.Services;
using Microsoft.AspNetCore.Authorization;


namespace GLTV.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{id?}")]
    public class TvItemsController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITvItemService _tvItemService;
        private readonly IEmailSender _emailSender;

        public TvItemsController(IFileService fileService, ITvItemService tvItemService, IEmailSender emailSender)
        {
            _fileService = fileService;
            _tvItemService = tvItemService;
            _emailSender = emailSender;
        }

        // GET: TvItems
        public async Task<IActionResult> Index()
        {
            List<TvItem> tvItems = _tvItemService.FetchTvItems(false);
            var model = new TvItemsViewModel();
            // these active items list contains items that were not yet played on tv screens
            model.ActiveTvItems = tvItems.Where(x => DateTime.Compare(DateTime.Now, x.EndTime) < 0).ToList();
            model.ExpiredTvItems = tvItems.Where(x => !model.ActiveTvItems.Select(y => y.ID).Contains(x.ID)).ToList();

            return View(model);
        }

        public async Task<IActionResult> IndexDeleted()
        {
            List<TvItem> tvItems = _tvItemService.FetchTvItems(true);

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
        public async Task<IActionResult> Create([Bind]TvItemCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // add item
                TvItem item = model.TvItem;
                item.Author = User.Identity.Name;
                item.TimeInserted = DateTime.Now;

                // add locations
                item.Locations = new List<TvItemLocation>();
                if (model.LocationCheckboxes.LocationBanskaBystrica)
                {
                    item.Locations.Add(new TvItemLocation() { Location = Location.BanskaBystrica });
                }

                if (model.LocationCheckboxes.LocationKosice)
                {
                    item.Locations.Add(new TvItemLocation() { Location = Location.Kosice });
                }

                if (model.LocationCheckboxes.LocationZilina)
                {
                    item.Locations.Add(new TvItemLocation() { Location = Location.Zilina });
                }

                _tvItemService.AddTvItem(item);

                // add files
                if (item.Type == TvItemType.Video)
                {
                    if (model.Files.Count > 1)
                    {
                        _tvItemService.DeleteTvItem(item.ID);
                        ModelState.AddModelError("", "Only 1 file is allowed for video TV item type.");
                        return View(model);
                    }

                    try
                    {
                        _fileService.SaveVideoFile(item, model.Files[0]);
                    }
                    catch (Exception e)
                    {
                        _tvItemService.DeleteTvItem(item.ID);
                        ModelState.AddModelError("", e.Message);
                        return View(model);
                    }
                }
                else
                {
                    try
                    {
                        _fileService.SaveImageFiles(item, model.Files);
                    }
                    catch (Exception e)
                    {
                        _tvItemService.DeleteTvItem(item.ID);
                        ModelState.AddModelError("", e.Message);
                        return View(model);
                    }
                }

                _emailSender.SendEmailAsync("jan.murin@globallogic.com", "gltv insert", "tvitem was inserted: " + item.Title);

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
            TvItem item = _tvItemService.FetchTvItem(model.TvItem.ID);

            if (ModelState.IsValid)
            {
                // update item
                item.Title = model.TvItem.Title;
                item.StartTime = model.TvItem.StartTime;
                item.EndTime = model.TvItem.EndTime;
                item.Duration = model.TvItem.Duration;

                // update locations
                item.Locations = new List<TvItemLocation>();
                if (model.LocationCheckboxes.LocationBanskaBystrica)
                {
                    item.Locations.Add(new TvItemLocation() { TvItemId = item.ID, Location = Location.BanskaBystrica });
                }

                if (model.LocationCheckboxes.LocationKosice)
                {
                    item.Locations.Add(new TvItemLocation() { TvItemId = item.ID, Location = Location.Kosice });
                }

                if (model.LocationCheckboxes.LocationZilina)
                {
                    item.Locations.Add(new TvItemLocation() { TvItemId = item.ID, Location = Location.Zilina });
                }

                _tvItemService.UpdateTvItem(item);

                return RedirectToAction(nameof(Index));
            }

            model.TvItem.Files = item.Files;

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

        [HttpPost, ActionName("DeleteFiles")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFiles(int id)
        {
            TvItem item = _tvItemService.FetchTvItem(id);

            _fileService.DeleteFiles(item.Files);

            return RedirectToAction(nameof(IndexDeleted));
        }
    }
}
