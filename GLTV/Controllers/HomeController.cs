using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Models.ViewModels;
using GLTV.Services;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

namespace GLTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;

        public HomeController(SignInManager<ApplicationUser> signInManager, IEmailSender emailSender
            , IUserService userService
            )
        {
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            //var contents = _fileProvider.GetDirectoryContents("");
            //return View(contents);
            return RedirectToAction("Index", "Inzeraty");
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            throw new Exception("test exception for email sending");

            //return View();
        }

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        public async Task<IActionResult> Error()
        {
            IExceptionHandlerFeature exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            ViewData["message"] = exception.Error.Message;
            ViewData["stackTrace"] = exception.Error.StackTrace;

            await _emailSender.SendEmailAsync(User.Identity.Name, EmailType.Error, exception);
            //await _eventService.AddWebServerLogAsync(
            //    User.Identity.Name,
            //    WebServerLogType.Exception,
            //    $"User encountered exception: [{exception.Error.Message}].", null);

            return View();
        }

        public async Task<IActionResult> Settings(bool emailNotification)
        {
            UserSetting setting = await _userService.FetchUserSettingAsync();

            var model = new SettingsViewModel();
            model.NotificationsEnabled = setting.NotificationsEnabled;

            return View("Settings", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSettings([Bind]SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserSetting setting = await _userService.FetchUserSettingAsync();
                setting.NotificationsEnabled = model.NotificationsEnabled;

                UserSetting userSetting = await _userService.UpdateUserSettingAsync(setting);
                model.NotificationsEnabled = userSetting.NotificationsEnabled;
            }

            return View("Settings", model);
        }
    }
}
