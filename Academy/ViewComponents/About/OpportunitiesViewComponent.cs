using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class OpportunitiesViewComponent : ViewComponent
    {
        private readonly ITopicService _topicService;

        public OpportunitiesViewComponent(ITopicService topicService)
        {
            _topicService = topicService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _topicService.GetAllAsync();
            return View(data);
        }
    }
}