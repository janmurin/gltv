using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Http;

namespace GLTV.Services
{
    public interface ITvItemService
    {
        Task<TvItem> FetchTvItemAsync(int id);
        Task<bool> DeleteTvItemAsync(int id);
        Task<List<TvItem>> FetchTvItemsAsync(bool deleted);
        Task<bool> AddTvItemAsync(TvItem item);
        Task<bool> UpdateTvItemAsync(TvItem item);
        Task<List<TvItem>> FetchActiveTvItemsAsync(Location location);
        Task<TvItemFile> FetchTvItemFileAsync(string filename);
        Task<TvItemFile> FetchTvItemFileAsync(int fileId);
    }
}
