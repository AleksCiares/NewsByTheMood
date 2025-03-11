using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Components
{
    // Dynamically articles topics nav bar 
    public class TopicsListViewComponent : ViewComponent
    {
        private readonly ITopicService _topicService;

        public TopicsListViewComponent(ITopicService topicService)
        {
            this._topicService = topicService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topics = (await this._topicService.GetAllAsync()) // replaced with mapper
                .Select(t => new TopicModel()
                {
                    Id = t.Id.ToString(),
                    Name = t.Name,
                    IconCssClass = t.IconCssClass
                })
                .ToArray();

            return View(topics);
        }
    }
}
