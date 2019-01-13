using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GLTV.Services
{
    public class ServiceBase
    {
        protected readonly ApplicationDbContext Context;

        private List<TvScreen> _knownScreens;

        public ServiceBase(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            Context = context;
            AllowedExtensions = new List<string> { "jpg", "jpe", "bmp", "jpeg", "png", "mkv", "mp4" };
            CurrentUser = signInManager.Context.User;
        }

        protected ClaimsPrincipal CurrentUser { get; }

        protected List<string> AllowedExtensions { get; }

        protected List<TvScreen> KnownTvScreens
        {
            get
            {
                if (_knownScreens == null)
                {
                    _knownScreens = Context.TvScreen.ToList();
                    return _knownScreens;
                }
                else
                {
                    return _knownScreens;
                }
            }
        }
    }
}
