using Academy.Data;
using Academy.Services.Interfaces;
using Academy.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Academy.ViewComponents.Course
{
    public class CourseCardViewComponent : ViewComponent
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;

        public CourseCardViewComponent(
            ICourseService courseService,
            ICategoryService categoryService,
            IInstructorService instructorService)
        {
            _courseService = courseService;
            _categoryService = categoryService;
            _instructorService = instructorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string? search,
            string? sort,
            int? categoryId,
            int? instructorId,
            decimal? minPrice,
            decimal? maxPrice)
        {
            var courses = await _courseService.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
                courses = courses.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || (x.CategoryName != null && x.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase)));

            if (categoryId != null)
                courses = courses.Where(x => x.CategoryId == categoryId);

            if (instructorId != null)
                courses = courses.Where(x => x.InstructorId == instructorId);

            if (minPrice != null)
                courses = courses.Where(x => x.Price >= minPrice);

            if (maxPrice != null)
                courses = courses.Where(x => x.Price <= maxPrice);

            if (sort == "latest")
                courses = courses.OrderByDescending(x => x.Id);
            else if (sort == "low")
                courses = courses.OrderBy(x => x.Price);
            else if (sort == "high")
                courses = courses.OrderByDescending(x => x.Price);

            var model = new CoursePageVM
            {
                Courses = courses.ToList(),
                Categories = (await _categoryService.GetAllAsync()).ToList(),
                Instructors = (await _instructorService.GetAllAsync()).ToList(),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CategoryId = categoryId,
                InstructorId = instructorId
            };

            return View(model);
        }
    }
}
