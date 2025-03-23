using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Components
{
    // Dynamically articles topics nav bar 
    public class TopicsListViewComponent : ViewComponent
    {
        private readonly ITopicService _topicService;
        private readonly ILogger<TopicsListViewComponent> _logger;

        public TopicsListViewComponent(ITopicService topicService, ILogger<TopicsListViewComponent> logger)
        {
            this._topicService = topicService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var topics = (await this._topicService.GetAllAsync()) // replaced with mapper
                    .Select(t => new TopicModel()
                    {
                        Name = t.Name,
                        IconCssClass = t.IconCssClass
                    })
                    .ToArray();

                return View(topics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching topics");
                return View(Array.Empty<TopicModel>());
            }
        }
    }
}
