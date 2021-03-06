﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;


namespace GLTV.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{id?}")]
    public class TvItemsController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITvItemService _tvItemService;
        private readonly IEmailSender _emailSender;
        private readonly IEventService _eventService;

        public TvItemsController(IFileService fileService, ITvItemService tvItemService, IEmailSender emailSender, IEventService eventService)
        {
            _fileService = fileService;
            _tvItemService = tvItemService;
            _emailSender = emailSender;
            _eventService = eventService;
        }

        // GET: TvItems
        public async Task<IActionResult> Index()
        {
            List<TvItem> tvItems = await _tvItemService.FetchTvItemsAsync(false);
            var model = new TvItemsViewModel();
            // these active items list contains items that were not yet played on tv screens
            model.ActiveTvItems = tvItems.Where(x => DateTime.Compare(DateTime.Now, x.EndTime) < 0).ToList();
            model.ExpiredTvItems = tvItems.Where(x => !model.ActiveTvItems.Select(y => y.ID).Contains(x.ID)).ToList();

            return View(model);
        }

        // GET: TvItems/Details/5
        public async Task<IActionResult> Details(int id)
        {
            TvItem item = await _tvItemService.FetchTvItemAsync(id, true);

            return View(item);
        }

        [AllowAnonymous]
        public async Task<IActionResult> DetailsAnonymous(int id)
        {
            TvItem item = await _tvItemService.FetchTvItemAsync(id, true);

            await _eventService.AddWebServerLogAsync(HttpContext.Connection.RemoteIpAddress.ToString(), WebServerLogType.AnonymousDetails, "", id);

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

                await _tvItemService.AddTvItemAsync(item);

                // add files
                if (item.Type == TvItemType.Video)
                {
                    if (model.Files.Count > 1)
                    {
                        await _tvItemService.DeleteTvItemAsync(item.ID);
                        ModelState.AddModelError("", "Only 1 file is allowed for video TV item type.");
                        return View(model);
                    }

                    try
                    {
                        await _fileService.SaveVideoFileAsync(item, model.Files[0]);
                    }
                    catch (Exception e)
                    {
                        await _tvItemService.DeleteTvItemAsync(item.ID);
                        ModelState.AddModelError("", e.Message);
                        return View(model);
                    }
                }
                else
                {
                    try
                    {
                        await _fileService.SaveImageFilesAsync(item, model.Files);
                    }
                    catch (Exception e)
                    {
                        await _tvItemService.DeleteTvItemAsync(item.ID);
                        ModelState.AddModelError("", e.Message);
                        return View(model);
                    }
                }

                await _emailSender.SendEmailAsync(item.Author, EmailType.Insert, item);
                await _eventService.AddWebServerLogAsync(User.Identity.Name, WebServerLogType.ItemInsert, "", item.ID);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = new TvItemEditViewModel(await _tvItemService.FetchTvItemAsync(id, true));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind]TvItemEditViewModel model)
        {
            TvItem item = await _tvItemService.FetchTvItemAsync(model.TvItem.ID);

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

                await _tvItemService.UpdateTvItemAsync(item);
                await _eventService.AddWebServerLogAsync(User.Identity.Name, WebServerLogType.ItemUpdate, "", item.ID);

                return RedirectToAction(nameof(Index));
            }

            // filter deleted files
            model.TvItem.Files = item.Files.Where(f => f.Deleted == false).ToList();

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            TvItem item = await _tvItemService.FetchTvItemAsync(id, true);

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tvItemService.DeleteTvItemAsync(id);
            await _eventService.AddWebServerLogAsync(User.Identity.Name, WebServerLogType.ItemDelete, "", id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFile(int id)
        {
            TvItemFile itemFile = await _tvItemService.FetchTvItemFileAsync(id);
            TvItem item = await _tvItemService.FetchTvItemAsync(itemFile.TvItemId);

            await _tvItemService.DeleteTvItemFileAsync(id);
            await _eventService.AddWebServerLogAsync(
                    User.Identity.Name,
                    WebServerLogType.ItemDeleteSingleFile,
                    $"User deleted file[{itemFile.FileName}] for item [{item.GetDetailHyperlink(false)}] with id [{item.ID}].",
                    item.ID);

            return RedirectToAction(nameof(Edit), new { id = item.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFiles(int id, [FromForm]List<IFormFile> files, [FromForm]int duration)
        {
            if (ModelState.IsValid)
            {
                // add item
                var model = new TvItemEditViewModel(await _tvItemService.FetchTvItemAsync(id));

                // add files
                try
                {
                    if (model.TvItem.Type == TvItemType.Video)
                    {
                        if (files.Count > 1)
                        {
                            ModelState.AddModelError("", "Only 1 file is allowed for video TV item type.");
                        }

                        if (duration < 3)
                        {
                            ModelState.AddModelError("", $"Incorrect video duration [{duration}]. Must be at least 4 seconds.");
                        }

                        if (ModelState.ErrorCount > 0)
                        {
                            return View("Edit", model);
                        }

                        model.TvItem.Duration = duration;
                        await _fileService.ReplaceVideoFileAsync(model.TvItem, files[0]);
                    }
                    else if (model.TvItem.Type == TvItemType.Image)
                    {
                        if (files.Count > 1)
                        {
                            ModelState.AddModelError("", "Only 1 file is allowed for image TV item type.");
                            return View("Edit", model);
                        }

                        await _fileService.ReplaceImageFileAsync(model.TvItem, files[0]);
                    }
                    else if (model.TvItem.Type == TvItemType.Gallery)
                    {
                        await _fileService.SaveImageFilesAsync(model.TvItem, files);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    // filter deleted files
                    model.TvItem.Files = model.TvItem.Files.Where(f => f.Deleted == false).ToList();
                    return View("Edit", model);
                }

                await _eventService.AddWebServerLogAsync(User.Identity.Name, WebServerLogType.ItemUpdate, "", model.TvItem.ID);

                //return RedirectToAction("Edit", new { id });
            }
            // todo: log model state error

            return RedirectToAction("Edit", new { id });
        }

        public async Task<IActionResult> ThrowException()
        {
            throw new Exception("thrown test exception");

            return RedirectToAction(nameof(Index));
        }
    }
}
