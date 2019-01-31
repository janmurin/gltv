using System.Collections.Generic;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Services
{
    public interface ITvItemService
    {
        Task<TvItem> FetchTvItemAsync(int id, bool filterFiles = false);
        Task<List<TvItem>> FetchTvItemsAsync(bool deleted);
        Task<List<TvItem>> FetchActiveTvItemsAsync(Location location);
        Task<TvItemFile> FetchTvItemFileAsync(string filename);
        Task<TvItemFile> FetchTvItemFileAsync(int fileId);

        Task<bool> AddTvItemAsync(TvItem item);
        Task<bool> UpdateTvItemAsync(TvItem item);

        Task DeleteTvItemAsync(int id);
        Task DeleteTvItemFileAsync(int fileId);
        Task DeleteTvItemFilesAsync(int tvItemId);
    }
}
