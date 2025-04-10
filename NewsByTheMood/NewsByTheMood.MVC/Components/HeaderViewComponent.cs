using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.MVC.Mappers;

namespace NewsByTheMood.MVC.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ITopicService _topicService;
        private readonly TopicsMapper _topicMapper;
        private readonly ILogger<HeaderViewComponent> _logger;

        public HeaderViewComponent(
            SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            ILogger<HeaderViewComponent> logger,
            TopicsMapper topicMapper,
            ITopicService topicService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _topicService = topicService;
            _topicMapper = topicMapper;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var topics = (await this._topicService.GetAllAsync()) // replaced with mapper
                    .Select(topic => _topicMapper.TopicToTopicModel(topic))
                    .ToArray();
                UserPreviewModel? user = null;

                if (_signInManager.IsSignedIn((ClaimsPrincipal)User))
                {
                    var userPrincipal = await _userManager.GetUserAsync((ClaimsPrincipal)User);
                    if (userPrincipal != null)
                    {
                        user = new UserPreviewModel
                        {
                            DisplayedName = userPrincipal.DisplayedName,
                            AvatarUrl = userPrincipal.AvatarUrl
                        };
                    }
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
                return View(false);
            }
        }
    }
}
