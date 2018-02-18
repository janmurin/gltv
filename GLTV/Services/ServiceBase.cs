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
        private readonly string _webRootPath;
        private readonly string _webPath;
        private readonly List<string> _allowedExtensions;
        protected readonly ApplicationDbContext _context;
        private ClaimsPrincipal _currentUser;

        public ServiceBase(ApplicationDbContext context, IHostingEnvironment env, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _webPath = "files";
            _webRootPath = Path.Combine(env.WebRootPath, _webPath);
            _allowedExtensions = new List<string> { "jpg", "jpe", "bmp", "jpeg", "png", "mkv", "mp4" };
            _currentUser = signInManager.Context.User;
        }

        protected ClaimsPrincipal CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }

        protected string WebRootPath
        {
            get
            {
                return _webRootPath;
            }
        }

        protected string WebPath
        {
            get
            {
                return _webPath;
            }
        }

        protected List<string> AllowedExtensions
        {
            get
            {
                return _allowedExtensions;
            }
        }

        protected string MakeWebPath(string filename)
        {
            return MakeWebPath(Path.Combine(WebPath,filename), true);
        }

        protected string MakeWebPath(string path, bool addSeperatorToBegin = false, bool addSeperatorToLast = false)
        {
            path = path.Replace("\\", "/");

            if (addSeperatorToBegin) path = "/" + path;
            if (addSeperatorToLast) path = path + "/";

            return path;
        }

        protected string MakeFullWebPath(string filename)
        {
            return string.Concat(Constants.SERVER_URL, MakeWebPath(filename));
        }
    }
}
