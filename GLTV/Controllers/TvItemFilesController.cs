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
    public class TvItemFilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TvItemFilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TvItemFiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.TvItemFile.ToListAsync());
        }

        // GET: TvItemFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItemFile = await _context.TvItemFile
                .SingleOrDefaultAsync(m => m.ID == id);
            if (tvItemFile == null)
            {
                return NotFound();
            }

            return View(tvItemFile);
        }

        // GET: TvItemFiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TvItemFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TvItemId,FileName,Duration,Length")] TvItemFile tvItemFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tvItemFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tvItemFile);
        }

        // GET: TvItemFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItemFile = await _context.TvItemFile.SingleOrDefaultAsync(m => m.ID == id);
            if (tvItemFile == null)
            {
                return NotFound();
            }
            return View(tvItemFile);
        }

        // POST: TvItemFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TvItemId,FileName,Duration,Length")] TvItemFile tvItemFile)
        {
            if (id != tvItemFile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tvItemFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TvItemFileExists(tvItemFile.ID))
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
            return View(tvItemFile);
        }

        // GET: TvItemFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvItemFile = await _context.TvItemFile
                .SingleOrDefaultAsync(m => m.ID == id);
            if (tvItemFile == null)
            {
                return NotFound();
            }

            return View(tvItemFile);
        }

        // POST: TvItemFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tvItemFile = await _context.TvItemFile.SingleOrDefaultAsync(m => m.ID == id);
            _context.TvItemFile.Remove(tvItemFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TvItemFileExists(int id)
        {
            return _context.TvItemFile.Any(e => e.ID == id);
        }
    }
}
