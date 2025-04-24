using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class IsExistsArticleByIdQueryHandler : IRequestHandler<IsExistsArticleByIdQuery, bool>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public IsExistsArticleByIdQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(IsExistsArticleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Id == request.Id)
                .AnyAsync(cancellationToken);
        }
    }
}
