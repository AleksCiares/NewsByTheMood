using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Services.FileProvider.Abstract;

namespace NewsByTheMood.MVC.Components
{
    public class TopicsIconsViewComponent : ViewComponent
    {
        private readonly IiconService _iconsService;
        private readonly ILogger<TopicsIconsViewComponent> _logger;

        public TopicsIconsViewComponent(IiconService iconsService, ILogger<TopicsIconsViewComponent> logger)
        {
            _iconsService = iconsService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                return View(await this._iconsService.GetIconsCssClassesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching icons css classes");
                return View(Array.Empty<string>());
            }
        }
    }
}
