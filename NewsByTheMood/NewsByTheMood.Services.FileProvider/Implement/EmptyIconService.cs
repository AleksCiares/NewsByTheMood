using NewsByTheMood.Services.FileProvider.Abstract;

namespace NewsByTheMood.Services.FileProvider.Implement
{
    public class EmptyIconService : IiconService
    {
        public async Task<string[]> GetIconsCssClassesAsync()
        {
            return await Task.FromResult(new List<string>().ToArray());
        }
    }
}
