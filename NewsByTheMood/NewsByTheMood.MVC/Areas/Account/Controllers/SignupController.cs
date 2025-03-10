using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;


namespace NewsByTheMood.MVC.Areas.Account.Controllers
{
    // Registration controller
    [Area("Account")]
    public class SignupController : Controller
    {
        private readonly IUserService _userService;

        public SignupController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm] SignupModel signup)
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
            return Json(!await _userService.IsUserNameExistsAsync(username));
        }

        // Check on existing email in db
        [HttpPost]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Json(!await _userService.IsUserNameExistsAsync(email));
        }
    }
}
