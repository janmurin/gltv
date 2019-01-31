using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;

namespace GLTV.Services
{
    public class FileService : ServiceBase, IFileService
    {
        public FileService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {
        }

        public Task<bool> SaveVideoFileAsync(TvItem tvItem, IFormFile file)
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

            if (tvItem.Duration == 0)
            {
                throw new Exception("Video duration is 0s.");
            }

            Context.Add(itemFile);
            Context.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> ReplaceVideoFileAsync(TvItem tvItem, IFormFile file)
        {
            string filename = tvItem.ID + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
            TvItemFile newItemFile = new TvItemFile()
            {
                FileName = filename,
                Length = file.Length
            };

            using (var fileStream = new FileStream(newItemFile.AbsolutePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            if (tvItem.Duration == 0)
            {
                throw new Exception("Video duration is 0s.");
            }

            TvItemFile tvItemFile = tvItem.Files.FirstOrDefault();
            if (tvItemFile != null)
            {
                bool success = DeletePhysicalFileAsync(tvItemFile.FileName).Result;
                // if not successful delete, then new zombie file 

                tvItemFile.FileName = newItemFile.FileName;
                tvItemFile.Length = newItemFile.Length;

                Context.Update(tvItemFile);
                Context.Update(tvItem);
                Context.SaveChanges();

                return Task.FromResult(true);
            }
            else
            {
                throw new Exception($"TvItem with id {tvItem.ID} does not have any files.");
            }
        }

        public Task<bool> SaveImageFilesAsync(TvItem item, List<IFormFile> modelFiles)
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

                    itemFile.Length = fileStream.Length;
                    Context.Add(itemFile);
                }
            }

            Context.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> ReplaceImageFileAsync(TvItem tvItem, IFormFile formFile)
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
            string filename = tvItem.ID + "_" + Guid.NewGuid() + extension;
            TvItemFile newItemFile = new TvItemFile()
            {
                FileName = filename,
                Length = formFile.Length,
                TvItemId = tvItem.ID
            };

            using (var fileStream = new FileStream(newItemFile.AbsolutePath, FileMode.Create))
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
                newItemFile.Length = fileStream.Length;
            }

            TvItemFile tvItemFile = tvItem.Files.FirstOrDefault();
            if (tvItemFile != null)
            {
                bool success = DeletePhysicalFileAsync(tvItemFile.FileName).Result;
                // if not successfull delete, then new zombie file 

                tvItemFile.FileName = newItemFile.FileName;
                tvItemFile.Length = newItemFile.Length;

                Context.Update(tvItemFile);
                Context.Update(tvItem);
                Context.SaveChanges();

                return Task.FromResult(true);
            }
            else
            {
                throw new Exception($"TvItem with id {tvItem.ID} does not have any files.");
            }
        }

        public Task<List<TvItemFile>> FindZombieFilesAsync()
        {
            DirectoryInfo d = new DirectoryInfo(Path.Combine(Constants.WEB_ROOT_PATH, Constants.FILES_DIR));
            FileInfo[] files = d.GetFiles();
            List<string> tvItemFiles = Context.TvItemFile.Select(x => x.FileName).ToList();

            List<TvItemFile> zombieFiles = new List<TvItemFile>();
            foreach (FileInfo file in files)
            {
                if (!tvItemFiles.Contains(file.Name) && !file.Name.Equals(".gitignore"))
                {
                    zombieFiles.Add(new TvItemFile() { FileName = file.Name, Length = file.Length });
                }
            }

            return Task.FromResult(zombieFiles);
        }

        /// <summary>
        /// Removing specific file. If the file does not exist, then it is still a success.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Task<bool> DeletePhysicalFileAsync(string filename)
        {
            TvItemFile file = new TvItemFile() { FileName = filename };

            try
            {
                if (File.Exists(file.AbsolutePath))
                {
                    File.Delete(file.AbsolutePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
