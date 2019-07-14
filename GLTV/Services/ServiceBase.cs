using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Identity;

namespace GLTV.Services
{
    public class ServiceBase
    {
        protected readonly ApplicationDbContext Context;
        protected readonly SignInManager<ApplicationUser> _signInManager;

        public ServiceBase(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            Context = context;
            AllowedExtensions = new List<string> { "jpg", "jpe", "bmp", "jpeg", "png", "mkv", "mp4" };
            _signInManager = signInManager;
        }

        protected ClaimsPrincipal CurrentUser
        {
            get
            {
                return _signInManager.Context.User;
            }
        }

        protected List<string> AllowedExtensions { get; }

    }
}
