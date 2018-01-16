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
using Microsoft.AspNetCore.Identity;

namespace GLTV.Services
{
    public class FileService : ServiceBase, IFileService
    {
        private readonly ITvItemService _tvItemService;

        public FileService(ApplicationDbContext context, IHostingEnvironment env, SignInManager<ApplicationUser> signInManager, ITvItemService tvItemService)
            : base(context, env, signInManager)
        {
            _tvItemService = tvItemService;
        }

        public Task<List<TvItemFile>> SaveFiles(int tvItemId, IEnumerable<IFormFile> files)
        {
            return Task.Run(async () =>
            {
                var result = new List<TvItemFile>();

                foreach (var file in files)
                {
                    // TODO: validation of extensions, video length and image size
                    //if (file.Length <= 0) continue;

                    string filename = tvItemId + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(WebRootPath, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    TvItemFile itemFile = new TvItemFile()
                    {
                        FileName = filename,
                        Length = file.Length,
                        TvItemId = tvItemId
                    };

                    _context.Add(itemFile);
                    result.Add(itemFile);
                }

                _context.SaveChanges();

                return result;
            });
        }

        public bool DeleteFile(string filename)
        {
            TvItemFile tvItemFile = _context.TvItemFile.SingleOrDefault(m => m.FileName.Equals(filename));
            if (tvItemFile == null)
            {
                throw new Exception($"TvItemFile not found with filename[{filename}].");
            }

            string path = Path.Combine(WebRootPath, filename);
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public bool DeleteFiles(int tvItemId)
        {
            List<TvItemFile> files = _tvItemService.FetchTvItem(tvItemId).Files;

            foreach (TvItemFile file in files)
            {
                string path = Path.Combine(WebRootPath, file.FileName);
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
            }

            return true;
        }

        public byte[] GetBytes(string filename)
        {
            throw new NotImplementedException();
        }


    }
}
