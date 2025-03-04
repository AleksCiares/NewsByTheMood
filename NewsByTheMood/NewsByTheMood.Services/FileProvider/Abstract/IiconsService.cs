using NewsByTheMood.MVC.Options;

namespace NewsByTheMood.Services.FileProvider.Abstract
{
    // Service interface for provide user icons for elements
    public interface IiconsService
    {
        // Get all icon css classes
        public Task<string[]> GetIconsCssClasses();
    }
}
