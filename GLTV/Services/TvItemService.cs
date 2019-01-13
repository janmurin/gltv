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

        public Task<TvItem> FetchTvItemAsync(int id)
        {
            var tvItem = Context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .SingleOrDefault(m => m.ID == id);

            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

            return Task.FromResult(tvItem);
        }

        public Task<bool> DeleteTvItemAsync(int id)
        {
            var tvItem = Context.TvItem.SingleOrDefault(m => m.ID == id);
            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

            // items are never actually deleted
            tvItem.Deleted = true;

            Context.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<List<TvItem>> FetchTvItemsAsync(bool deleted)
        {
            List<TvItem> tvItems = Context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == deleted)
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            return Task.FromResult(tvItems);
        }

        public Task<bool> AddTvItemAsync(TvItem item)
        {
            Context.Add(item);
            Context.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> UpdateTvItemAsync(TvItem item)
        {
            Context.Update(item);
            Context.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<List<TvItem>> FetchActiveTvItemsAsync(Location location)
        {
            List<TvItem> tvItems = Context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == false && x.Locations.Select(y => y.Location).Contains(location))
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            tvItems = tvItems.Where(x => DateTime.Compare(DateTime.Now, x.StartTime) > 0 && DateTime.Compare(DateTime.Now, x.EndTime) < 0).ToList();

            return Task.FromResult(tvItems);
        }

        public Task<TvItemFile> FetchTvItemFileAsync(string filename)
        {
            TvItemFile itemFile = Context.TvItemFile.FirstOrDefault(x => x.FileName.Equals(filename));
            
            return Task.FromResult(itemFile);
        }

        public Task<TvItemFile> FetchTvItemFileAsync(int fileId)
        {
            TvItemFile itemFile = Context.TvItemFile.FirstOrDefault(x => x.ID == fileId);
            if (itemFile == null)
            {
                throw new Exception($"Item not found with fileId {fileId}.");
            }

            return Task.FromResult(itemFile);
        }
    }
}
