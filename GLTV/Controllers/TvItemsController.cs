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
using Microsoft.Extensions.FileProviders;

namespace GLTV.Controllers
{
    [Authorize]
    public class TvItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private IHostingEnvironment _env;

        public TvItemsController(ApplicationDbContext context, IFileService fileService, IHostingEnvironment env)
        {
            _context = context;
            _fileService = fileService;
            _env = env;
        }

        // GET: TvItems
        public async Task<IActionResult> Index()
        {
            List<TvItem> tvItems = await _context.TvItem.ToListAsync();
            List<TvItemLocation> tvItemLocations = await _context.TvItemLocation.ToListAsync();
            foreach (TvItem tvItem in tvItems)
            {
                tvItem.Locations = tvItemLocations.Where(x => x.TvItemId == tvItem.ID).ToList();
            }

            return View(tvItems);
        }

        // GET: TvItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItem = await _context.TvItem
                .SingleOrDefaultAsync(m => m.ID == id);
            if (tvItem == null)
            {
                return NotFound();
            }

            return View(tvItem);
        }

        // GET: TvItems/Create
        public IActionResult Create()
        {
            return View("Create", new TvItemEditViewModel());
        }

        // POST: TvItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> Create([Bind]TvItemEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                // add item
                TvItem item = model.TvItem;
                item.Author = User.Identity.Name;
                item.TimeInserted = DateTime.Now;
                _context.Add(item);
                _context.SaveChanges();

                // add locations
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
                _context.AddRange(model.TvItem.Locations);
                _context.SaveChanges();

                // add files
                _fileService.SaveFiles(item.ID, model.Files);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: TvItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItem = await _context.TvItem.SingleOrDefaultAsync(m => m.ID == id);
            if (tvItem == null)
            {
                return NotFound();
            }
            return View(tvItem);
        }

        // POST: TvItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,TimeInserted,StartTime,EndTime,Author,Duration,Type")] TvItem tvItem)
        {
            if (id != tvItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tvItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TvItemExists(tvItem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tvItem);
        }

        // GET: TvItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItem = await _context.TvItem
                .SingleOrDefaultAsync(m => m.ID == id);
            if (tvItem == null)
            {
                return NotFound();
            }

            return View(tvItem);
        }

        // POST: TvItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tvItem = await _context.TvItem.SingleOrDefaultAsync(m => m.ID == id);
            _context.TvItem.Remove(tvItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TvItemExists(int id)
        {
            return _context.TvItem.Any(e => e.ID == id);
        }
    }
}
