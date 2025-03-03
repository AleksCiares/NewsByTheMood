using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Services.FileProvider.Abstract;

namespace NewsByTheMood.MVC.Components
{
    public class TopicsIconViewComponent : ViewComponent
    {
        private readonly IiconsService? _iconsService;

        public TopicsIconViewComponent(IiconsService? iconsService)
        {
            _iconsService = iconsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var iconCssClasses = new List<string>();
            if (this._iconsService != null)
            {
                var temp = await this._iconsService.GetIconsCssClasses();
                if (temp != null)
                {
                    iconCssClasses.AddRange(temp);
                }
            }

            return View(iconCssClasses.ToArray());
        }
    }
}
