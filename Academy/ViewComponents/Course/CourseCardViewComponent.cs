using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Course
{
    public class CourseCardViewComponent : ViewComponent
    {
        private readonly ICourseService _service;

        public CourseCardViewComponent(ICourseService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = await _service.GetAllAsync(); 
            return View(courses);
        }
    }
}
