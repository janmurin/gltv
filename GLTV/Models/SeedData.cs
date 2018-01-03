using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GLTV.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.TvItem.Any())
                {
                    return;   // DB has been seeded
                }

                context.TvItem.AddRange(
                    new TvItem()
                    {
                        Author = "jan.murin",
                        Duration = 20,
                        EndTime = DateTime.Now.AddDays(20),
                        StartTime = DateTime.Now,
                        TimeInserted = DateTime.Now,
                        Title = "first image gallery",
                        Type = TvItemType.Gallery
                    },
                    new TvItem()
                    {
                        Author = "jan.murin",
                        Duration = 60,
                        EndTime = DateTime.Now.AddDays(30),
                        StartTime = DateTime.Now,
                        TimeInserted = DateTime.Now,
                        Title = "first image",
                        Type = TvItemType.Image
                    },
                    new TvItem()
                    {
                        Author = "jan.murin",
                        Duration = 120,
                        EndTime = DateTime.Now.AddDays(10),
                        StartTime = DateTime.Now,
                        TimeInserted = DateTime.Now,
                        Title = "first video",
                        Type = TvItemType.Video
                    }
                );
                context.TvItemFile.AddRange(
                    new TvItemFile()
                    {
                        TvItemId = 1,
                        FileName = "image1.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = 1,
                        FileName = "image2.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = 1,
                        FileName = "image3.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = 2,
                        FileName = "first-image.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = 3,
                        FileName = "first-video.mp4",
                        Length = 500000
                    }
                );

                context.TvItemLocation.AddRange(
                    new TvItemLocation()
                    {
                        TvItemId = 1,
                        Location = Location.Kosice
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 1,
                        Location = Location.BanskaBystrica
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 1,
                        Location = Location.Zilina
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 2,
                        Location = Location.Kosice
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 2,
                        Location = Location.BanskaBystrica
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 2,
                        Location = Location.Zilina
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 3,
                        Location = Location.Kosice
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 3,
                        Location = Location.BanskaBystrica
                    },
                    new TvItemLocation()
                    {
                        TvItemId = 3,
                        Location = Location.Zilina
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
