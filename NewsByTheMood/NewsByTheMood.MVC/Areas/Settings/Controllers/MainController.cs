using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Articles controller
    [Area("Settings")]
    [Route("Settings/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
