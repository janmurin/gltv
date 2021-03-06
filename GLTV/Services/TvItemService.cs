﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GLTV.Services
{
    public class TvItemService : ServiceBase, ITvItemService
    {
        private readonly IFileService _fileService;

        public TvItemService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IFileService fileService)
            : base(context, signInManager)
        {
            _fileService = fileService;
        }

        public Task<TvItem> FetchTvItemAsync(int id, bool filterFiles = false)
        {
            var tvItem = Context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .SingleOrDefault(m => m.ID == id);

            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }
            // do not filter files: filtering files out causes unwanted delete sqls,
            // because context is tracking the whole object with its related entities
            if (filterFiles)
            {
                tvItem.Files = tvItem.Files.Where(f => f.Deleted == false).ToList();
            }

            return Task.FromResult(tvItem);
        }

        public Task<List<TvItem>> FetchTvItemsAsync(bool deleted)
        {
            // used only for read only purposes, no danger regarding updates/deletes
            List<TvItem> tvItems = Context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == deleted)
                .OrderByDescending(x => x.TimeInserted)
                .AsNoTracking()
                .ToList();

            tvItems.ForEach(tv =>
            {
                tv.Files = tv.Files.Where(f => f.Deleted == false).ToList();
            });

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
            // used only for read only purposes for API, no danger regarding updates/deletes
            List<TvItem> tvItems = Context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == false && x.Locations.Select(y => y.Location).Contains(location))
                .OrderByDescending(x => x.TimeInserted)
                .AsNoTracking()
                .ToList();

            tvItems = tvItems.Where(x => DateTime.Compare(DateTime.Now, x.StartTime) > 0 && DateTime.Compare(DateTime.Now, x.EndTime) < 0).ToList();
            tvItems.ForEach(tv =>
            {
                tv.Files = tv.Files.Where(f => f.Deleted == false).ToList();
            });

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

        public Task DeleteTvItemAsync(int tvItemId)
        {
            var tvItem = FetchTvItemAsync(tvItemId).Result;

            // items are never actually deleted
            tvItem.Deleted = true;
            Context.TvItem.Update(tvItem);
            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteTvItemFileAsync(int fileId)
        {
            TvItemFile tvItemFile = Context.TvItemFile.SingleOrDefault(m => m.ID == fileId);
            if (tvItemFile == null)
            {
                throw new Exception($"Item not found with id {fileId}.");
            }
            Console.WriteLine($"deleting file: {tvItemFile.FileName}");

            _fileService.DeletePhysicalFileAsync(tvItemFile.AbsolutePath);
            tvItemFile.Deleted = true;

            Context.TvItemFile.Update(tvItemFile);
            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteTvItemFilesAsync(int tvItemId)
        {
            TvItem tvItem = FetchTvItemAsync(tvItemId).Result;
            Console.WriteLine("deleting files: " + String.Join(", ", tvItem.Files.Select(x => x.FileName)));

            foreach (TvItemFile file in tvItem.Files)
            {
                _fileService.DeletePhysicalFileAsync(file.AbsolutePath);
                file.Deleted = true;
                Context.TvItemFile.Update(file);
            }

            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task DeleteAllUndeletedFilesAsync()
        {
            // get all items which are deleted
            List<TvItem> tvItems = Context.TvItem
                .Include(x => x.Files)
                .Include(y => y.Locations)
                .Where(x => x.Deleted == true)
                .ToList();

            List<TvItemFile> toDeleteFiles = new List<TvItemFile>();
            tvItems.ForEach(tv =>
            {
                // from deleted items get all files which are not deleted
                toDeleteFiles.AddRange(tv.Files.Where(f => f.Deleted == false).ToList());
            });

            toDeleteFiles.ForEach(f =>
            {
                _fileService.DeletePhysicalFileAsync(f.AbsolutePath);
                f.Deleted = true;

                Context.TvItemFile.Update(f);
            });

            Context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
