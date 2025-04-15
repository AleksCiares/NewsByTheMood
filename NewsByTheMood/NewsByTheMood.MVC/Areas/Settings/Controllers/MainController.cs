using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Core.Settings;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Articles controller
    [Area("Settings")]
    [Authorize(Roles = AccessLevels.Admininistrator)]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
