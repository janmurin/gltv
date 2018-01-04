using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                    return; // DB has been seeded
                }
                TvItem item1 = context.TvItem.Add(
                    new TvItem()
                    {
                        Author = "jan.murin",
                        Duration = 20,
                        EndTime = DateTime.Now.AddDays(20),
                        StartTime = DateTime.Now,
                        TimeInserted = DateTime.Now,
                        Title = "first image gallery",
                        Type = TvItemType.Gallery
                    }).Entity;

                TvItem item2 = context.TvItem.Add(
                    new TvItem()
                    {
                        Author = "jan.murin",
                        Duration = 60,
                        EndTime = DateTime.Now.AddDays(30),
                        StartTime = DateTime.Now,
                        TimeInserted = DateTime.Now,
                        Title = "first image",
                        Type = TvItemType.Image
                    }).Entity;

                TvItem item3 = context.TvItem.Add(
                    new TvItem()
                    {
                        Author = "jan.murin",
                        Duration = 120,
                        EndTime = DateTime.Now.AddDays(10),
                        StartTime = DateTime.Now,
                        TimeInserted = DateTime.Now,
                        Title = "first video",
                        Type = TvItemType.Video
                    }).Entity;

                context.TvItemFile.AddRange(
                    new TvItemFile()
                    {
                        TvItemId = item1.ID,
                        FileName = "image1.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = item1.ID,
                        FileName = "image2.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = item1.ID,
                        FileName = "image3.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = item2.ID,
                        FileName = "first-image.jpg",
                        Length = 500000
                    },
                    new TvItemFile()
                    {
                        TvItemId = item3.ID,
                        FileName = "first-video.mp4",
                        Length = 500000
                    }
                );

                context.TvItemLocation.AddRange(
                    new TvItemLocation()
                    {
                        TvItemId = item1.ID,
                        Location = Location.Kosice
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item1.ID,
                        Location = Location.BanskaBystrica
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item1.ID,
                        Location = Location.Zilina
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item2.ID,
                        Location = Location.Kosice
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item2.ID,
                        Location = Location.BanskaBystrica
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item2.ID,
                        Location = Location.Zilina
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item3.ID,
                        Location = Location.Kosice
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item3.ID,
                        Location = Location.BanskaBystrica
                    },
                    new TvItemLocation()
                    {
                        TvItemId = item3.ID,
                        Location = Location.Zilina
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
