using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _topicService.GetByIdAsync(id);
            if (data == null) return NotFound();

            var model = new TopicEditVM
            {
                Id = data.Id,
                Title = data.Title,
                SubTitle = data.SubTitle
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TopicEditVM model)
        {


            var topic = new Topic
            {
                Id = model.Id,
                Title = model.Title,
                SubTitle = model.SubTitle
            };

            await _topicService.UpdateAsync(topic);

            return RedirectToAction(nameof(Index));
        }


    }
}