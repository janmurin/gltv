﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using Xabe.FFmpeg;

namespace GLTV.Services
{
    public class FileService : ServiceBase, IFileService
    {
        public FileService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {
        }

        public bool SaveVideoFile(TvItem tvItem, IFormFile file)
        {
            string filename = tvItem.ID + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
            TvItemFile itemFile = new TvItemFile()
            {
                FileName = filename,
                Length = file.Length,
                TvItemId = tvItem.ID
            };

            using (var fileStream = new FileStream(itemFile.AbsolutePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            IMediaInfo mediaInfo = new MediaInfo(itemFile.AbsolutePath);
            tvItem.Duration = (int)mediaInfo.Properties.VideoDuration.TotalSeconds;
            if (tvItem.Duration == 0)
            {
                throw new Exception("Video duration is 0s.");
            }

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
                double k1 = width / Constants.MAX_IMAGE_WIDTH;
                double k2 = height / Constants.MAX_IMAGE_HEIGHT;
                if (k1 > k2 && k1 > 1)
                {
                    image.Mutate(x => x.Resize(Constants.MAX_IMAGE_WIDTH, (int)(height / k1)));
                }

                if (k1 < k2 && k2 > 1)
                {
                    image.Mutate(x => x.Resize((int)(width / k2), Constants.MAX_IMAGE_HEIGHT));
                }

                string extension = Path.GetExtension(formFile.FileName) ?? "";
                string filename = item.ID + "_" + Guid.NewGuid() + extension;
                TvItemFile itemFile = new TvItemFile()
                {
                    FileName = filename,
                    Length = formFile.Length,
                    TvItemId = item.ID
                };

                using (var fileStream = new FileStream(itemFile.AbsolutePath, FileMode.Create))
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

            try
            {
                if (File.Exists(tvItemFile.AbsolutePath))
                {
                    File.Delete(tvItemFile.AbsolutePath);
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
                try
                {
                    if (File.Exists(file.AbsolutePath))
                    {
                        File.Delete(file.AbsolutePath);
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
