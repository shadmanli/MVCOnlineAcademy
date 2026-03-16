using Academy.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.ViewComponents.Home
{
    public class NumbersViewComponent:ViewComponent
    {
        private readonly IStatisticService _statisticService;
        private readonly IMapper _mapper;
        public NumbersViewComponent(IStatisticService statisticService,IMapper mapper)
        {
            _mapper = mapper;
            _statisticService = statisticService;
        }
        public  async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _statisticService.GetAllAsync();
            return View(data);
        }
    }
}
