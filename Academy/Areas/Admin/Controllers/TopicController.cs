using Academy.Services.Interfaces;
using Academy.ViewModels.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

      
        public async Task<IActionResult> Index()
        {
            var data = await _topicService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(TopicCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _topicService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

     
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _topicService.GetByIdAsync(id);

            if (data == null)
                return NotFound();

            return View(data);
        }

    
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _topicService.DeleteAsync(id);
            return Ok();
        }
    }
}
