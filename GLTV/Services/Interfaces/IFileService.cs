using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Http;

namespace GLTV.Services
{
    public interface IFileService
    {
        Task<bool> SaveVideoFileAsync(TvItem tvItemId, IFormFile files);
        Task<bool> ReplaceVideoFileAsync(TvItem tvItem, IFormFile file);
        Task<bool> DeleteFileAsync(string filename);
        Task<bool> DeleteFilesAsync(List<TvItemFile> files);
        byte[] GetBytes(string filename);
        Task<bool> SaveImageFilesAsync(TvItem item, List<IFormFile> modelFiles);
        Task<bool> ReplaceImageFileAsync(TvItem tvItem, IFormFile formFile);
        Task<List<TvItemFile>> FindZombieFilesAsync();
        Task<bool> DeleteZombieFileAsync(string filename);
    }
}
