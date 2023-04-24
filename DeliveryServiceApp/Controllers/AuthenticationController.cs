﻿using DeliveryServiceApp.Models;
using DeliveryServiceDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeliveryServiceApp.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<Person> userManager;
        private readonly SignInManager<Person> signInManager;

        public AuthenticationController(UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #region registration
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel model)
        {
            Customer customer = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                PostalCode = model.PostalCode
            };

            if(string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.PasswordConfirm) || !model.Password.Equals(model.PasswordConfirm))
            {
                ModelState.AddModelError("Password", "Password not added");
                return View();
            }

            var result = await userManager.CreateAsync(customer, model.Password);

            if (result.Succeeded)
            {
                var currentUser = await userManager.FindByNameAsync(customer.UserName);

                var roleresult = await userManager.AddToRoleAsync(currentUser, "User");

                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                if(result.Errors.Any(e => e.Code.Contains("DuplicateUserName")))
                {
                    ModelState.AddModelError("Username", result.Errors.FirstOrDefault(e => e.Code == "DuplicateUserName")?.Description);
                }
                if (result.Errors.Any(e => e.Code.Contains("Password")))
                {
                    ModelState.AddModelError("Password", result.Errors.FirstOrDefault(e => e.Code.Contains("Password"))?.Description);
                }
                return View();
            }
        }
        #endregion

        #region login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains("User"))
                {
                    HttpContext.Session.SetString("userrole", "User");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Wrong credentials!");
            return View();
        }
        #endregion

        #region logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}
