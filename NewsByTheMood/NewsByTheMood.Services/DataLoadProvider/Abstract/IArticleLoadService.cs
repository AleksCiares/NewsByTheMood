using NewsByTheMood.Data.Entities;
namespace NewsByTheMood.Services.DataLoadProvider.Abstract
{
    /// <summary>
    /// Service for load articles to database
    /// </summary>
    public interface IArticleLoadService
    {
        public Task LoadArticles(Source source);
    }
}
