using Microsoft.AspNetCore.Mvc;

namespace NewsByTheMood.MVC.Components
{
    public class AdminMenuPanelViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
