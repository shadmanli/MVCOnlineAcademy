using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Slider
{
    public class SliderViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
