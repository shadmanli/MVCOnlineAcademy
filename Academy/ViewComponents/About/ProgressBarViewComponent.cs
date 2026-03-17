using Academy.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class ProgressBarViewComponent:ViewComponent
    {
        private readonly IMissionService _missionService;
        private readonly IMapper _mapper;
        public ProgressBarViewComponent(IMissionService missionService,IMapper mapper)
        {
            _mapper = mapper;
            _missionService = missionService;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
            var mission = (await _missionService.GetAllAsync()).FirstOrDefault();
            return View(mission);
        }
    }
}
