﻿// <auto-generated />
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace GLTV.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("GLTV.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GLTV.Models.Objects.Inzerat", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Category");

                    b.Property<DateTime>("DateInserted");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("Location");

                    b.Property<string>("Phone");

                    b.Property<string>("Portal");

                    b.Property<string>("Price");

                    b.Property<string>("Title");

                    b.Property<string>("Type");

                    b.Property<string>("Url");

                    b.HasKey("ID");

                    b.ToTable("Inzerat");
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<bool>("Deleted");

                    b.Property<int>("Duration");

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<DateTime>("TimeInserted");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.ToTable("TvItem");
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvItemFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<long>("Length");

                    b.Property<int>("TvItemId");

                    b.HasKey("ID");

                    b.HasIndex("TvItemId");

                    b.ToTable("TvItemFile");
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvItemLocation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Location");

                    b.Property<int>("TvItemId");

                    b.HasKey("ID");

                    b.HasIndex("TvItemId");

                    b.ToTable("TvItemLocation");
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvScreen", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(300);

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("LastHandshake");

                    b.Property<int>("Location");

                    b.HasKey("ID");

                    b.ToTable("TvScreen");
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvScreenHandshake", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("FirstHandshake");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("LastHandshake");

                    b.Property<int>("TvScreenId");

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.HasIndex("TvScreenId");

                    b.ToTable("TvScreenHandshake");
                });

            modelBuilder.Entity("GLTV.Models.Objects.WebClientLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message")
                        .HasMaxLength(500);

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("TimeInserted");

                    b.Property<int?>("TvItemFileId");

                    b.Property<int?>("TvScreenId");

                    b.Property<int>("Type")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.HasIndex("TvItemFileId");

                    b.HasIndex("TvScreenId");

                    b.ToTable("WebClientLog");
                });

            modelBuilder.Entity("GLTV.Models.WebServerLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Message")
                        .HasMaxLength(500);

                    b.Property<DateTime>("TimeInserted");

                    b.Property<int?>("TvItemId");

                    b.Property<int>("Type")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.HasIndex("TvItemId");

                    b.ToTable("WebServerLog");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvItemFile", b =>
                {
                    b.HasOne("GLTV.Models.Objects.TvItem")
                        .WithMany("Files")
                        .HasForeignKey("TvItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvItemLocation", b =>
                {
                    b.HasOne("GLTV.Models.Objects.TvItem")
                        .WithMany("Locations")
                        .HasForeignKey("TvItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GLTV.Models.Objects.TvScreenHandshake", b =>
                {
                    b.HasOne("GLTV.Models.Objects.TvScreen", "TvScreen")
                        .WithMany()
                        .HasForeignKey("TvScreenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GLTV.Models.Objects.WebClientLog", b =>
                {
                    b.HasOne("GLTV.Models.Objects.TvItemFile", "TvItemFile")
                        .WithMany()
                        .HasForeignKey("TvItemFileId");

                    b.HasOne("GLTV.Models.Objects.TvScreen", "TvScreen")
                        .WithMany()
                        .HasForeignKey("TvScreenId");
                });

            modelBuilder.Entity("GLTV.Models.WebServerLog", b =>
                {
                    b.HasOne("GLTV.Models.Objects.TvItem", "TvItem")
                        .WithMany()
                        .HasForeignKey("TvItemId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GLTV.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GLTV.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GLTV.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GLTV.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
