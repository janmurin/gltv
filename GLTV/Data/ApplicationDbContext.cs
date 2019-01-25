using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GLTV.Models;
using GLTV.Models.Objects;

namespace GLTV.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<TvItem> TvItem { get; set; }

        public DbSet<TvItemFile> TvItemFile { get; set; }

        public DbSet<TvItemLocation> TvItemLocation { get; set; }

        public DbSet<GLTV.Models.LogEvent> LogEvent { get; set; }

        public DbSet<GLTV.Models.ClientEvent> ClientEvent { get; set; }
    }
}
