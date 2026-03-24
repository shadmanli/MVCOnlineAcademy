using Academy.Services.Interfaces;
using Academy.ViewModels.ImpactItem;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Teacher
{
    public class GlobalViewComponent:ViewComponent
    {
        private readonly IImpactItemService _itemService;
        private readonly IImpactSectionService _sectionService;
        public GlobalViewComponent(IImpactSectionService impactSectionService,IImpactItemService impactItemService)
        {
            _itemService = impactItemService;
            _sectionService = impactSectionService;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {

            var sections = await _sectionService.GetAllAsync();
            return View(sections);
        }
    }
}
