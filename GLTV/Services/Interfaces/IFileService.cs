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
        bool SaveVideoFile(TvItem tvItemId, IFormFile files);
        bool ReplaceVideoFile(TvItem tvItem, IFormFile file);
        bool DeleteFile(string filename);
        bool DeleteFiles(List<TvItemFile> files);
        byte[] GetBytes(string filename);
        bool SaveImageFiles(TvItem item, List<IFormFile> modelFiles);
        bool ReplaceImageFile(TvItem tvItem, IFormFile formFile);
        List<TvItemFile> FindZombieFiles();
        bool DeleteZombieFile(string filename);
    }
}
