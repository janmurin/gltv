﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GLTV.Models;
using GLTV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

namespace GLTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileProvider _fileProvider;
        private readonly IEmailSender _emailSender;

        public HomeController(SignInManager<ApplicationUser> signInManager, IFileProvider fileProvider, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _fileProvider = fileProvider;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            var contents = _fileProvider.GetDirectoryContents("");
            return View(contents);
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            throw new Exception("test exception for email sending");

            return View();
        }

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        public IActionResult Error()
        {
            IExceptionHandlerFeature exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            ViewData["message"] = exception.Error.Message;
            ViewData["stackTrace"] = exception.Error.StackTrace;

            _emailSender.SendEmailAsync(User.Identity.Name, EmailType.Error, exception);

            return View();
        }
    }
}
