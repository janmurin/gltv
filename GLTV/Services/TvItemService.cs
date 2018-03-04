using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
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
            var tvItem = _context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .SingleOrDefault(m => m.ID == id);

            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

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
            List<TvItem> tvItems = _context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == deleted)
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            foreach (TvItem tvItem in tvItems)
            {
                tvItem.Files.ForEach(i => i.Url = MakeWebPath(i.FileName));
                tvItem.Files.ForEach(i => i.AbsolutePath = Path.Combine(WebRootPath, i.FileName));
            }

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

            tvItems.Where(x => DateTime.Compare(DateTime.Now, x.StartTime) > 0 && DateTime.Compare(DateTime.Now, x.EndTime) < 0).ToList();

            foreach (TvItem tvItem in tvItems)
            {
                // special ending to differentiate between web and client file requests
                tvItem.Files.ForEach(i => i.FullUrl = MakeFullWebPath(i.FileName) + Constants.CLIENT_FILE_REQUEST_SUFFIX);
                tvItem.Files.ForEach(i => i.AbsolutePath = Path.Combine(WebRootPath, i.FileName));
            }

            return tvItems;
        }

        public TvItemFile GetTvItemFile(string filename)
        {
            return _context.TvItemFile.FirstOrDefault(x => x.FileName.Equals(filename));
        }
    }
}
