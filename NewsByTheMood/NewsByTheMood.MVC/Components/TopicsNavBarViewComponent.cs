using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Components
{
    // Dynamically articles topics nav bar 
    public class TopicsNavBarViewComponent : ViewComponent
    {
        private readonly ITopicService _topicService;

        public TopicsNavBarViewComponent(ITopicService topicService)
        {
            this._topicService = topicService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topics = (await this._topicService.GetAll())? // replaced with mapper
                .Select(t => new TopicModel()
                {
                    TopicName = t.Name
                })
                .ToArray();
            return View(topics);
        }
    }
}
