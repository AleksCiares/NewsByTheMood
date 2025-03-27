using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, Article?>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetArticleByIdQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article?> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _dbContext.Articles
               .Where(article => article.Id == request.Id)
               .SingleOrDefaultAsync(cancellationToken);

            if (article != null)
            {
                await _dbContext.Entry(article)
                    .Reference(article => article.Source)
                    .LoadAsync(cancellationToken);

                await _dbContext.Entry(article.Source)
                    .Reference(source => source.Topic)
                    .LoadAsync(cancellationToken);

                await _dbContext.Entry(article)
                    .Collection(article => article.Tags)
                    .LoadAsync(cancellationToken);

                _dbContext.Entry(article)
                    .State = EntityState.Detached;

                return article;
            }
            else
            {
                return null;
            }
        }
    }
}

/*return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Id == id)
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .Include(article => article.Tags)
                .SingleOrDefaultAsync();*/