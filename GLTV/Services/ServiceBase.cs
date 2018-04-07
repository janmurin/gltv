using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
using GLTV.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GLTV.Services
{
    public class ServiceBase
    {
        protected readonly ApplicationDbContext _context;

        public ServiceBase(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            AllowedExtensions = new List<string> { "jpg", "jpe", "bmp", "jpeg", "png", "mkv", "mp4" };
            CurrentUser = signInManager.Context.User;
        }

        protected ClaimsPrincipal CurrentUser { get; }

        protected List<string> AllowedExtensions { get; }
    }
}
