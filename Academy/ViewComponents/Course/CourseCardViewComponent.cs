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
            string? level,
            decimal? minPrice,
            decimal? maxPrice,
            int page = 1)
        {
            var courses = await _courseService.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
                courses = courses.Where(x => (x.Title != null && x.Title.Contains(search, StringComparison.OrdinalIgnoreCase)) || (x.CategoryName != null && x.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase)));

            if (categoryId != null)
                courses = courses.Where(x => x.CategoryId == categoryId);

            if (instructorId != null)
                courses = courses.Where(x => x.InstructorId == instructorId);
                
            if (!string.IsNullOrEmpty(level))
                courses = courses.Where(x => !string.IsNullOrEmpty(x.Level) &&
                    x.Level.Equals(level, StringComparison.OrdinalIgnoreCase));

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
            else if (sort == "rating")
                courses = courses.OrderByDescending(x => x.Rating);
            else if (sort == "popular")
                courses = courses.OrderByDescending(x => x.StudentCount);

            int pageSize = 6;
            int totalItems = courses.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Pagination execution
            var pagedCourses = courses.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new CoursePageVM
            {
                Courses = pagedCourses,
                Categories = (await _categoryService.GetAllAsync()).ToList(),
                Instructors = (await _instructorService.GetAllAsync()).ToList(),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CategoryId = categoryId,
                InstructorId = instructorId,
                Level = level,
                CurrentPage = page,
                TotalPages = totalPages,
                Search = search,
                Sort = sort
            };

            return View(model);
        }
    }
}
