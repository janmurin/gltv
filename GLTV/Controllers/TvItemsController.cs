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
    public class TvItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TvItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TvItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.TvItem.ToListAsync());
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
            return View();
        }

        // POST: TvItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,TimeInserted,StartTime,EndTime,Author,Duration,Type")] TvItem tvItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tvItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tvItem);
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
