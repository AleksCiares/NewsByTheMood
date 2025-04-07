using MediatR;
using Microsoft.Extensions.Logging;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Implement;
using NSubstitute;

namespace NewsByTheMood.Services.Tests
{
    public class ArticleServiceTests
    {
        private readonly Article[] _articlesArray = new Article[]
        {
            new Article { Id = 1, Positivity = 5 },
            new Article { Id = 2, Positivity = 7 },
            new Article { Id = 3, Positivity = 3 },
            new Article { Id = 4, Positivity = 8 },
            new Article { Id = 5, Positivity = 6 },
            new Article { Id = 6, Positivity = 4 },
            new Article { Id = 7, Positivity = 9 },
            new Article { Id = 8, Positivity = 2 },
            new Article { Id = 9, Positivity = 1 },
            new Article { Id = 10, Positivity = 10 }
        };

        [Fact]
        public async Task AddRangeAsync_CheckArticleExists_ReturnBoolean()
        {
            // Arrange
            var mediatorMock = Substitute.For<IMediator>();
            await mediatorMock.Send(Arg.Any<AddArticlesRangeCommand>(), Arg.Any<CancellationToken>());

            var loggerMock = Substitute.For<ILogger<ArticleService>>();

            var articleService = new ArticleService(mediatorMock, loggerMock);

            // Act
            var result = await articleService.AddRangeAsync(_articlesArray);

            // Assert
            Assert.True(result);
        }

        // functional tests for other methods
    }
}