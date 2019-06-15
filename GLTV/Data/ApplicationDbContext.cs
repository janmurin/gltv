﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<Inzerat> Inzerat { get; set; }
        public DbSet<UserFilter> UserFilter { get; set; }
        public DbSet<IgnoredInzerat> IgnoredInzerat { get; set; }
        public DbSet<MarkedInzerat> MarkedInzerat { get; set; }
        public DbSet<Filter> Filter { get; set; }
        public DbSet<ScraperLogEvent> ScraperLogEvent { get; set; }
    }
}
