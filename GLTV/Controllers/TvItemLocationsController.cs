using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GLTV.Data;
using GLTV.Models;

namespace GLTV.Controllers
{
    public class TvItemLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TvItemLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TvItemLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.TvItemLocation.ToListAsync());
        }

        // GET: TvItemLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItemLocation = await _context.TvItemLocation
                .SingleOrDefaultAsync(m => m.ID == id);
            if (tvItemLocation == null)
            {
                return NotFound();
            }

            return View(tvItemLocation);
        }

        // GET: TvItemLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TvItemLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TvItemId")] TvItemLocation tvItemLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tvItemLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tvItemLocation);
        }

        // GET: TvItemLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItemLocation = await _context.TvItemLocation.SingleOrDefaultAsync(m => m.ID == id);
            if (tvItemLocation == null)
            {
                return NotFound();
            }
            return View(tvItemLocation);
        }

        // POST: TvItemLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TvItemId")] TvItemLocation tvItemLocation)
        {
            if (id != tvItemLocation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tvItemLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TvItemLocationExists(tvItemLocation.ID))
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
            return View(tvItemLocation);
        }

        // GET: TvItemLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItemLocation = await _context.TvItemLocation
                .SingleOrDefaultAsync(m => m.ID == id);
            if (tvItemLocation == null)
            {
                return NotFound();
            }

            return View(tvItemLocation);
        }

        // POST: TvItemLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tvItemLocation = await _context.TvItemLocation.SingleOrDefaultAsync(m => m.ID == id);
            _context.TvItemLocation.Remove(tvItemLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TvItemLocationExists(int id)
        {
            return _context.TvItemLocation.Any(e => e.ID == id);
        }
    }
}
