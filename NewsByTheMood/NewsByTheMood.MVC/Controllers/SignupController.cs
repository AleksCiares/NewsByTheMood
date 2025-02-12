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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm]SignupModel signup)
        {
            var isUsernameExists = true;
            var isEmailExists = true;

            if (!ModelState.IsValid ||
                (isUsernameExists = await this._userService.IsUserNameExists(signup.Username)) ||
                (isEmailExists = await this._userService.IsEmailExists(signup.Email)))
            {
                return View(signup);
            }

            // add user to databse

            return RedirectToAction("Index", "Home");
        }

        // Check on existing username in db
        [NonAction]
        private async Task<IActionResult> CheckUserName(string username) 
        {
            return Json(!await this._userService.IsUserNameExists(username));
        }

        // Check on existing email in db
        [NonAction]
        private async Task<IActionResult> CheckEmail(string email)
        {
            return Json(!await this._userService.IsUserNameExists(email));
        }
    }
}
