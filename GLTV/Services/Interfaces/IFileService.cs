using System.Collections.Generic;
using System.Threading.Tasks;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Http;

namespace GLTV.Services
{
    public interface IFileService
    {
        Task<bool> SaveVideoFileAsync(TvItem tvItemId, IFormFile files);
        Task<bool> ReplaceVideoFileAsync(TvItem tvItem, IFormFile file);

        Task<bool> SaveImageFilesAsync(TvItem item, List<IFormFile> modelFiles);
        Task<bool> ReplaceImageFileAsync(TvItem tvItem, IFormFile formFile);

        Task<List<TvItemFile>> FindZombieFilesAsync();

        Task<bool> DeletePhysicalFileAsync(string filename);
    }
}
