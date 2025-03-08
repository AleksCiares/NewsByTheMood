using NewsByTheMood.Services.FileProvider.Abstract;

namespace NewsByTheMood.Services.FileProvider.Implement
{
    public class EmptyIconService : IiconService
    {
        public async Task<string[]> GetIconsCssClasses()
        {
            return new List<string>().ToArray();
        }
    }
}
