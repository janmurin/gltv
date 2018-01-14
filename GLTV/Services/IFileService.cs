using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GLTV.Services
{
    public interface IFileService
    {
        List<string> SaveFiles(int tvItemId, IEnumerable<IFormFile> files);
        bool DeleteFile(string filename);
        bool DeleteFiles(int tvItemId);
        byte[] GetBytes(string filename);
    }
}
