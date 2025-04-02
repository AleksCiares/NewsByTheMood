using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public UpdateArticleCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingArticle = await _dbContext.Articles
                    .Include(article => article.Tags)
                    .SingleOrDefaultAsync(article => article.Id == request.Article.Id, cancellationToken);

                if (existingArticle == null)
                {
                    throw new Exception($"Article with ID {request.Article.Id} not found.");
                }

                // Обновление тегов
                var newTagIds = request.Article.Tags.Select(t => t.Id).ToList();
                var currentTagIds = existingArticle.Tags.Select(t => t.Id).ToList();

                // Удаление тегов, которые больше не указаны
                var tagsToRemove = existingArticle.Tags.Where(t => !newTagIds.Contains(t.Id)).ToList();
                foreach (var tag in tagsToRemove)
                {
                    existingArticle.Tags.Remove(tag);
                }

                // Добавление новых тегов
                var tagsToAdd = newTagIds.Where(id => !currentTagIds.Contains(id)).ToList();
                foreach (var tagId in tagsToAdd)
                {
                    var tag = await _dbContext.Tags.FindAsync(new object[] { tagId }, cancellationToken);
                    if (tag != null)
                    {
                        existingArticle.Tags.Add(tag);
                    }
                }

                // Обновление остальных полей статьи
                existingArticle.Title = request.Article.Title;
                existingArticle.Url = request.Article.Url;
                existingArticle.PreviewImgUrl = request.Article.PreviewImgUrl;
                existingArticle.Body = request.Article.Body;
                existingArticle.PublishDate = request.Article.PublishDate;
                existingArticle.Positivity = request.Article.Positivity;
                existingArticle.Rating = request.Article.Rating;
                existingArticle.IsActive = request.Article.IsActive;
                existingArticle.FailedLoaded = request.Article.FailedLoaded;
                existingArticle.SourceId = request.Article.SourceId;

                _dbContext.Articles.Update(existingArticle);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("A concurrency error occurred while updating the article.", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception("An error occurred while updating the article.", ex);
            }
        }
    }
}
