﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;


namespace NewsByTheMood.MVC.Controllers
{
    // Registration controller
    public class SignupController : Controller
    {
        private readonly IUserService _userService;

        public SignupController(IUserService userService)
        {
            this._userService = userService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public  IActionResult Index([FromForm]SignupModel signup)
        {
            if (!ModelState.IsValid)
            {
                return View(signup);
            }

            // add user to databse

            return RedirectToAction("Index", "Home");
        }

        // Check on existing username in db
        [HttpPost]
        public async Task<IActionResult> CheckUserName(string username) 
        {
            return Json(!await this._userService.IsUserNameExistsAsync(username));
        }

        // Check on existing email in db
        [HttpPost]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Json(!await this._userService.IsUserNameExistsAsync(email));
        }
    }
}
