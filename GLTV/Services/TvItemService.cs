using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GLTV.Services
{
    public class TvItemService : ServiceBase, ITvItemService
    {
        private readonly IFileService _fileService;

        public TvItemService(ApplicationDbContext context, IHostingEnvironment env, SignInManager<ApplicationUser> signInManager, IFileService fileService)
            : base(context, env, signInManager)
        {
            _fileService = fileService;
        }

        public TvItem FetchTvItem(int id)
        {
            var tvItem = _context.TvItem.SingleOrDefault(m => m.ID == id);
            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

            tvItem.Locations = _context.TvItemLocation.ToList().Where(x => x.TvItemId == tvItem.ID).ToList();
            tvItem.Files = _context.TvItemFile.ToList().Where(x => x.TvItemId == tvItem.ID).ToList();
            tvItem.Files.ForEach(i => i.Url = MakeWebPath(i.FileName));
            tvItem.Files.ForEach(i => i.AbsolutePath = Path.Combine(WebRootPath, i.FileName));

            return tvItem;
        }

        public bool DeleteTvItem(int id)
        {
            var tvItem = _context.TvItem.SingleOrDefault(m => m.ID == id);
            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

            // items are never actually deleted
            tvItem.Deleted = true;

            _context.SaveChanges();

            return true;
        }

        public List<TvItem> FetchTvItems(bool deleted)
        {
            List<TvItem> tvItems = _context.TvItem.Where(x => x.Deleted == deleted).OrderByDescending(x => x.TimeInserted).ToList();
            List<TvItemLocation> tvItemLocations = _context.TvItemLocation.ToList();
            List<TvItemFile> tvItemFiles = _context.TvItemFile.ToList();

            foreach (TvItem tvItem in tvItems)
            {
                tvItem.Locations = tvItemLocations.Where(x => x.TvItemId == tvItem.ID).ToList();
                tvItem.Files = tvItemFiles.Where(x => x.TvItemId == tvItem.ID).ToList();
                tvItem.Files.ForEach(i => i.Url = MakeWebPath(i.FileName));
                tvItem.Files.ForEach(i => i.AbsolutePath = Path.Combine(WebRootPath, i.FileName));
            }

            return tvItems;
        }

        public bool AddTvItem(TvItem item, List<TvItemLocation> tvItemLocations)
        {
            _context.Add(item);

            foreach (TvItemLocation itemLocation in tvItemLocations)
            {
                itemLocation.TvItemId = item.ID;
            }

            _context.AddRange(tvItemLocations);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateTvItem(TvItem item, List<TvItemLocation> tvItemLocations)
        {
            _context.Update(item);

            List<TvItemLocation> itemLocations = _context.TvItemLocation.Where(m => m.TvItemId == item.ID).ToList();
            _context.TvItemLocation.RemoveRange(itemLocations);

            _context.AddRange(tvItemLocations);
            _context.SaveChanges();

            return true;
        }

    }
}
