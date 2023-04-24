﻿using DeliveryServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryServiceApp.Controllers
{
    public class HomeController : Controller
    { 
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccesDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error(string message)
        {
            ErrorViewModel model = new ErrorViewModel { Message = message };
            return View(model);
        }
    }
}
