using Academy.Services.Interfaces;
using Academy.ViewModels.Statistic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticController : Controller
    {
        private readonly IStatisticService _statisticService;
        private readonly IMapper _mapper;
        public StatisticController(IStatisticService statisticService, IMapper mapper)
        {
            _mapper = mapper;
            _statisticService = statisticService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _statisticService.GetAllAsync();

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(StatisticCreateVM model)
        {
            await _statisticService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(int id)
        {
            var data = await _statisticService.GetByIdAsync(id);
            if (data == null) return NotFound();
            var statisticVM = _mapper.Map<StatisticDetailVM>(data);
            return View(statisticVM);
        }


        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _statisticService.GetByIdAsync(id);
            if (data == null) return NotFound();
            await _statisticService.DeleteAsync(data);
            return Ok();
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _statisticService.GetByIdAsync(id);
            if (data == null) return NotFound();

            var editVM = _mapper.Map<StatisticEditVM>(data);
            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StatisticEditVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var statistic = await _statisticService.GetByIdAsync(model.Id);
            if (statistic == null) return NotFound();

            // Mapping vasitəsi ilə dəyişiklikləri tətbiq et
            _mapper.Map(model, statistic);
            await _statisticService.UpdateAsync(statistic);

            return RedirectToAction(nameof(Index));
        }
    }
}