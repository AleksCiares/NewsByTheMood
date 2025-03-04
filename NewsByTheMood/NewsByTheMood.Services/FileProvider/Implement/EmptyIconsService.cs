using NewsByTheMood.Services.FileProvider.Abstract;

namespace NewsByTheMood.Services.FileProvider.Implement
{
    public class EmptyIconsService : IiconsService
    {
        public async Task<string[]> GetIconsCssClasses()
        {
            return new List<string>().ToArray();
        }
    }
}
