using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;
using Microsoft.AspNetCore.Http;

namespace GLTV.Services
{
    public interface IFileService
    {
        Task<List<TvItemFile>> SaveFiles(int tvItemId, IEnumerable<IFormFile> files);
        bool DeleteFile(string filename);
        bool DeleteFiles(List<TvItemFile> files);
        byte[] GetBytes(string filename);
    }
}
