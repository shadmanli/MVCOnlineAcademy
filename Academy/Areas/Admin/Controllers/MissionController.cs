using Academy.Services.Interfaces;
using Academy.ViewModels.Mission;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SuperAdmin,Admin,Muellim")]
    public class MissionController : Controller
    {
        private readonly IMissionService _missionService;
        private readonly IMapper _mapper;
        public MissionController(IMissionService missionService, IMapper mapper)
        {
            _mapper = mapper;
            _missionService = missionService;

        }
        public async Task<IActionResult> Index()
        {
            var data = await _missionService.GetAllAsync();
            return View(data);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(MissionCreateVM mission)
        {

            await _missionService.CreateAsync(mission);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Detail(int id)
        {
            var data = await _missionService.GetByIdAsync(id);
            if (data == null) return NotFound();
            var missionVM = _mapper.Map<MissionDetailVM>(data);
            return View(missionVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _missionService.GetByIdAsync(id);
            if (data == null) return NotFound();
            await _missionService.DeleteAsync(data);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _missionService.GetByIdAsync(id);
            if (data == null) return NotFound();

            var editVM = _mapper.Map<MissionEditVM>(data);
            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MissionEditVM mission)
        {
          

            await _missionService.UpdateAsync(mission);
            return RedirectToAction(nameof(Index));
        }
    }
}
