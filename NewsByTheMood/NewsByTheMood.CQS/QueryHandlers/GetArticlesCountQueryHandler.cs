using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetArticlesCountQueryHandler : IRequestHandler<GetArticlesCountQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetArticlesCountQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }   

        public async Task<int> Handle(GetArticlesCountQuery request, CancellationToken cancellationToken)
        {
            /*if (request.Positivity < 0 || request.Positivity > 10)
            {
                throw new Exception($"Positivity must be greater than or equal to 0 and less than 10. " +
                    $"Positivity: {request.Positivity}");
            }*/
            /*if (request.TopicIds != null && request.TopicIds.Count() == 0)
            {
                throw new Exception($"TopicIds count must be greater than 0 or TopicIds must be null. " +
                    $"TopicIds count: {request.TopicIds.Count()}");
            }*/

            var query = _dbContext.Articles
                 .AsNoTracking()
                 .Where(article => article.Positivity >= request.Positivity);

            if (!request.IgnoreActivity)
            {
                query = query.Where(article => article.IsActive);
            }

            if (request.TopicIds != null && request.TopicIds.Count() > 0)
            {
                if (request.TopicIds.Count() == 1)
                {
                    query = query.Where(article => article.Source.TopicId == request.TopicIds.ElementAt(0));
                }
                else
                {
                    query = query.Where(article => request.TopicIds.Contains(article.Source.TopicId));
                }
            }

            return await query.CountAsync(cancellationToken);
        }
    }
}
