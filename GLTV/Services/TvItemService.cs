using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GLTV.Services
{
    public class TvItemService : ServiceBase, ITvItemService
    {
        public TvItemService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {
        }

        public TvItem FetchTvItem(int id)
        {
            var tvItem = _context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .SingleOrDefault(m => m.ID == id);

            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

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
            List<TvItem> tvItems = _context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == deleted)
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            return tvItems;
        }

        public bool AddTvItem(TvItem item)
        {
            _context.Add(item);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateTvItem(TvItem item)
        {
            _context.Update(item);
            _context.SaveChanges();

            return true;
        }

        public List<TvItem> FetchActiveTvItems(Location location)
        {
            List<TvItem> tvItems = _context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == false && x.Locations.Select(y => y.Location).Contains(location))
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            tvItems = tvItems.Where(x => DateTime.Compare(DateTime.Now, x.StartTime) > 0 && DateTime.Compare(DateTime.Now, x.EndTime) < 0).ToList();

            return tvItems;
        }

        public TvItemFile FetchTvItemFile(string filename)
        {
            TvItemFile itemFile = _context.TvItemFile.FirstOrDefault(x => x.FileName.Equals(filename));
            if (itemFile == null)
            {
                throw new Exception($"Item not found with filename {filename}.");
            }

            return itemFile;
        }

        public TvItemFile FetchTvItemFile(int fileId)
        {
            TvItemFile itemFile = _context.TvItemFile.FirstOrDefault(x => x.ID == fileId);
            if (itemFile == null)
            {
                throw new Exception($"Item not found with fileId {fileId}.");
            }

            return itemFile;
        }
    }
}
