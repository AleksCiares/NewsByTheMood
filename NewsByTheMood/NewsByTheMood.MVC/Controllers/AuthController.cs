using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;


namespace NewsByTheMood.MVC.Controllers
{
    // Registration controller
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            this._userService = userService;
        }
        
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup([FromForm]SignupModel signup)
        {
            if (!ModelState.IsValid)
            {
                return View(signup);
            }

            //var isEmailExistTask = this._userService.IsEmailExists(signup.Email);
            //var isUserNameExistTask = this._userService.IsUserNameExists(signup.UserName);

            return RedirectToAction("Index", "Home");
        }
    }
}
