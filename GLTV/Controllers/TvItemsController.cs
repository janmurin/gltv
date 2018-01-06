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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace GLTV.Controllers
{
    public class TvItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private IHostingEnvironment _env;

        public TvItemsController(ApplicationDbContext context, IFileProvider fileProvider, IHostingEnvironment env)
        {
            _context = context;
            _fileProvider = fileProvider;
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
        public async Task<IActionResult> Create([Bind]TvItemEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model.TvItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            //var stream = System.IO.File.Create("/files/file");
            List<string> paths = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string path = _env.WebRootPath + "/files/" + formFile.FileName;
                    paths.Add(path);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return RedirectToAction(nameof(Create));
        }

        //public async Task<IActionResult> Download(string filename)
        //{
        //    if (filename == null)
        //        return Content("filename not present");

        //    var path = Path.Combine(
        //        Directory.GetCurrentDirectory(),
        //        "wwwroot", filename);

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, GetContentType(path), Path.GetFileName(path));
        //}

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
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
