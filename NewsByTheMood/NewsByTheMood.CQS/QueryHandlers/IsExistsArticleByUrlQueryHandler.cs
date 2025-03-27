using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class IsExistsArticleByUrlQueryHandler : IRequestHandler<IsExistsArticleByUrlQuery, bool>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public IsExistsArticleByUrlQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(IsExistsArticleByUrlQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Url.Equals(request.ArticleUrl))
                .AnyAsync(cancellationToken);
        }
    }
}
