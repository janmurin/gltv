using System.Collections.Generic;
using System.Security.Claims;
using GLTV.Data;
using GLTV.Models;
using Microsoft.AspNetCore.Identity;

namespace GLTV.Services
{
    public class ServiceBase
    {
        protected readonly ApplicationDbContext Context;

        public ServiceBase(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            Context = context;
            AllowedExtensions = new List<string> { "jpg", "jpe", "bmp", "jpeg", "png", "mkv", "mp4" };
            CurrentUser = signInManager.Context.User;
        }

        protected ClaimsPrincipal CurrentUser { get; }

        protected List<string> AllowedExtensions { get; }
    }
}
