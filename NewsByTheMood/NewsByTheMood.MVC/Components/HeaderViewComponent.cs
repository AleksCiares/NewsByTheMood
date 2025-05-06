using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.Mappers;
using NewsByTheMood.Services.MVC.Mappers;

namespace NewsByTheMood.MVC.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ITopicService _topicService;
        private readonly TopicsMapper _topicsMapper;
        private readonly IUserService _userService;
        private readonly UsersMapper _usersMapper;
        private readonly ILogger<HeaderViewComponent> _logger;

        public HeaderViewComponent(
            ITopicService topicService,
            TopicsMapper topicMapper,
            IUserService userService,
            UsersMapper usersMapper,
            ILogger<HeaderViewComponent> logger)
        {
            _topicService = topicService;
            _topicsMapper = topicMapper;
            _userService = userService;
            _usersMapper = usersMapper;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var topics = (await _topicService.GetAllAsync())
                    .Select(topic => _topicsMapper.TopicToTopicModel(topic))
                    .ToArray();

                UserPreviewModel? user = null;
                if (HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    user = _usersMapper.UserToUserPreviewModel(await _userService.GetUserAsync(HttpContext.User));
                }

                return View(new HeaderModel() 
                {
                    UserPreview = user,
                    Topics = topics 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HeaderViewComponent");
                return View(new HeaderModel()
                { 
                    UserPreview = null,
                    Topics = Array.Empty<TopicModel>()
                });
            }
        }
    }
}
