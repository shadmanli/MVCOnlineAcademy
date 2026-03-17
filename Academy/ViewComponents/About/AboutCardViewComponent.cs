using Academy.Services.Interfaces;
using Academy.ViewModels.FeatureVM;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.About
{
    public class AboutCardViewComponent : ViewComponent
    {
        private readonly IFeatureService _featureService;
        private readonly IMapper _mapper;
        public AboutCardViewComponent(IFeatureService featureService,IMapper mapper)
        {
            _featureService = featureService;
            _mapper = mapper;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
            var data = await _featureService.GetAllAsync();
                var featureVMs = _mapper.Map<IEnumerable<FeatureVM>>(data);
               
            return View(featureVMs);
        }
    } 
}
