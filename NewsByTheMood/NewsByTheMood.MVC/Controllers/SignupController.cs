using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup([FromForm]SignupModel signup)
        {
            if(!ModelState.IsValid)
            {
                return View(signup);
            }

            var isUsernameExists = await this._userService.IsUserNameExists(signup.Username);
            var isEmailExists= await this._userService.IsEmailExists(signup.Email);
            if (isUsernameExists || isEmailExists)
            {
                return View(signup);
            }

            return RedirectToAction("Index", "Home");
        }

        // Check on existing username in db
        [HttpPost]
        public async Task<IActionResult> CheckUserName([FromForm]string username) 
        {
            return Json(!await this._userService.IsUserNameExists(username));
        }

        // Check on existing email in db
        [HttpPost]
        public async Task<IActionResult> CheckEmail([FromForm]string email)
        {
            return Json(!await this._userService.IsUserNameExists(email));
        }
    }
}
