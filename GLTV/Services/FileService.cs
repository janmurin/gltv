using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
using GLTV.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using Xabe.FFmpeg;

namespace GLTV.Services
{
    public class FileService : ServiceBase, IFileService
    {
        public FileService(ApplicationDbContext context, IHostingEnvironment env, SignInManager<ApplicationUser> signInManager)
            : base(context, env, signInManager)
        {
        }

        public bool SaveVideoFile(TvItem tvItem, IFormFile file)
        {
            string filename = tvItem.ID + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(WebRootPath, filename);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            IMediaInfo mediaInfo = new MediaInfo(path);
            tvItem.Duration = (int)mediaInfo.Properties.VideoDuration.TotalSeconds;
            if (tvItem.Duration == 0)
            {
                throw new Exception("Video duration is 0s.");
            }

            TvItemFile itemFile = new TvItemFile()
            {
                FileName = filename,
                Length = file.Length,
                TvItemId = tvItem.ID
            };

            _context.Add(itemFile);
            _context.SaveChanges();

            return true;
        }

        public bool SaveImageFiles(TvItem item, List<IFormFile> modelFiles)
        {
            foreach (IFormFile formFile in modelFiles)
            {
                Image<Rgba32> image = null;
                Stream inputStream = formFile.OpenReadStream();

                image = Image.Load(inputStream);
                double height = image.Height;
                double width = image.Width;

                if (width > Constants.MAX_IMAGE_WIDTH)
                {
                    double k = width / Constants.MAX_IMAGE_WIDTH;
                    image.Mutate(x => x.Resize(Constants.MAX_IMAGE_WIDTH, (int)(height / k)));
                }
                height = image.Height;
                if (height > Constants.MAX_IMAGE_HEIGHT)
                {
                    double k = height / Constants.MAX_IMAGE_HEIGHT;
                    image.Mutate(x => x.Resize((int)(width / k), Constants.MAX_IMAGE_HEIGHT));
                }

                string extension = Path.GetExtension(formFile.FileName) ?? "";
                string filename = item.ID + "_" + Guid.NewGuid() + extension;
                string path = Path.Combine(WebRootPath, filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    if (extension.ToLower().EndsWith("jpg") || extension.ToLower().EndsWith("jpeg"))
                    {
                        image.SaveAsJpeg(fileStream);
                    }
                    else if (extension.ToLower().EndsWith("png"))
                    {
                        image.SaveAsPng(fileStream);
                    }
                    else
                    {
                        throw new Exception($"Unsupported image file extension [{extension}].");
                    }

                    TvItemFile itemFile = new TvItemFile()
                    {
                        FileName = filename,
                        Length = formFile.Length,
                        TvItemId = item.ID
                    };

                    _context.Add(itemFile);
                }
            }

            _context.SaveChanges();

            return true;
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

        public bool DeleteFiles(List<TvItemFile> files)
        {
            foreach (TvItemFile file in files)
            {
                Console.WriteLine("deleting files: " + String.Join(", ", files.Select(x => x.FileName)));
                string path = Path.Combine(WebRootPath, file.FileName);
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    file.Deleted = true;
                    _context.TvItemFile.Update(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
            }

            _context.SaveChanges();

            return true;
        }

        public byte[] GetBytes(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
