using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class AddArticlesRangeCommandHandler : IRequestHandler<AddArticlesRangeCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public AddArticlesRangeCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddArticlesRangeCommand request, CancellationToken cancellationToken)
        {
            var tags = new Dictionary<string, Tag>();

            foreach (var article in request.Articles)
            {
                var tagNames = article.Tags.Select(tag => tag.Name).ToList().Distinct().ToList();
                article.Tags = new();

                foreach (var tagName in tagNames)
                {
                    if (!tags.ContainsKey(tagName))
                    {
                        var tag = await _dbContext.Tags.SingleOrDefaultAsync(tag => tag.Name.Equals(tagName), cancellationToken);
                        if (tag == null)
                        {
                            tag = new Tag() { Name = tagName };
                            await _dbContext.Tags.AddAsync(tag);
                        }
                        tags.Add(tagName, tag);
                    }

                    article.Tags.Add(tags[tagName]);
                }
            }

            await _dbContext.Articles.AddRangeAsync(request.Articles, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
