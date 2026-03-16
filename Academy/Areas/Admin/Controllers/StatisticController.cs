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
    }
}