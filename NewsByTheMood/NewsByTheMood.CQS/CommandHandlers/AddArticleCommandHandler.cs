using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.CommandHandlers
{
    public class AddArticleCommandHandler : IRequestHandler<AddArticleCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public AddArticleCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {
            var tagNames = request.Article.Tags.Select(tag => tag.Name).ToList().Distinct().ToList();
            request.Article.Tags = new();

            await _dbContext.Articles.AddAsync(request.Article);
            foreach (var tagName in tagNames)
            {
                var tag = await _dbContext.Tags.SingleOrDefaultAsync(tag => tag.Name.Equals(tagName), cancellationToken);
                if (tag == null)
                {
                    tag = new Tag() { Name = tagName };
                    await _dbContext.Tags.AddAsync(tag);
                }
                request.Article.Tags.Add(tag);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
