using Academy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.ViewComponents.Teacher
{
    public class InstructorViewComponent : ViewComponent
    {
        private readonly IInstructorService _instructorService;

        public InstructorViewComponent(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var all = (await _instructorService.GetAllAsync()).ToList();
            const int pageSize = 8;
            int totalPages = (int)Math.Ceiling(all.Count / (double)pageSize);
            var paged = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages  = totalPages;
            ViewBag.TotalCount  = all.Count;

            return View(paged);
        }
    }
}
