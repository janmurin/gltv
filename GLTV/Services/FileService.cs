using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GLTV.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _webRootPath;
        private readonly string _webPath;
        private readonly List<string> _allowedExtensions;

        public FileService(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _webPath = "files";
            _webRootPath = Path.Combine(env.WebRootPath, _webPath);
            _allowedExtensions = new List<string> { "jpg", "jpe", "jpeg", "gif", "png", "avi", "mkv", "mp4" };
        }

        public List<string> SaveFiles(int tvItemId, IEnumerable<IFormFile> files)
        {
            var result = new List<string>();

            foreach (var file in files)
            {
                // TODO: validation of extensions and video length
                //if (file.Length <= 0) continue;

                string filename = tvItemId + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);

                using (var fileStream = new FileStream(Path.Combine(_webRootPath, filename), FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }

                _context.Add(new TvItemFile()
                {
                    FileName = filename,
                    Length = file.Length,
                    TvItemId = tvItemId
                });

                result.Add(filename);
            }

            _context.SaveChanges();

            return result;
        }

        public bool DeleteFile(string filename)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFiles(int tvItemId)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes(string filename)
        {
            throw new NotImplementedException();
        }

        private static string MakeWebPath(string path, bool addSeperatorToBegin = false, bool addSeperatorToLast = false)
        {
            path = path.Replace("\\", "/");

            if (addSeperatorToBegin) path = "/" + path;
            if (addSeperatorToLast) path = path + "/";

            return path;
        }
    }
}
