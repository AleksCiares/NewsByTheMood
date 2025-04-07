using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetSourceByIdQueryHandler : IRequestHandler<GetSourceByIdQuery, Source?>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetSourceByIdQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Source?> Handle(GetSourceByIdQuery request, CancellationToken cancellationToken)
        {
            var source = await _dbContext.Sources
                .Where(source => source.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (source != null)
            {
                await _dbContext.Entry(source)
                    .Reference(source => source.Topic)
                .LoadAsync(cancellationToken);

                await _dbContext.Entry(source)
                    .Collection(source => source.Articles)
                .LoadAsync(cancellationToken);

                _dbContext.Entry(source)
                    .State = EntityState.Detached;

                return source;
            }
            else
            {
                return null;
            }
        }
    }
}