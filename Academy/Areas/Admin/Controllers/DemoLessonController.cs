using Academy.Data;
using Academy.Models;
using Academy.ViewModels.DemoLesson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Academy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DemoLessonController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DemoLessonController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var lessons = await _context.DemoLessons.Include(d => d.Course).ToListAsync();
            return View(lessons);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new DemoLessonCreateVM
            {
                Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DemoLessonCreateVM model)
        {
            if (model.VideoFile == null || model.ThumbnailFile == null || model.CourseId == 0)
            {
                ModelState.AddModelError("", "Bütün fayllar? v? aid oldu?u kursu seçin.");
                model.Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync();
                return View(model);
            }

            var demo = new DemoLesson
            {
                Title = model.Title,
                Description = model.Description,
                Duration = model.Duration,
                CourseId = model.CourseId
            };

            // Save Video
            string vidFolder = Path.Combine(_env.WebRootPath, "uploads/demovideos");
            if (!Directory.Exists(vidFolder)) Directory.CreateDirectory(vidFolder);
            string vidName = Guid.NewGuid() + "_" + model.VideoFile.FileName;
            string vidPath = Path.Combine(vidFolder, vidName);
            using (var vidStream = new FileStream(vidPath, FileMode.Create))
            {
                await model.VideoFile.CopyToAsync(vidStream);
            }
            demo.VideoUrl = vidName;

            // Save Thumbnail
            string thumbFolder = Path.Combine(_env.WebRootPath, "uploads/demothumbnails");
            if (!Directory.Exists(thumbFolder)) Directory.CreateDirectory(thumbFolder);
            string thumbName = Guid.NewGuid() + "_" + model.ThumbnailFile.FileName;
            string thumbPath = Path.Combine(thumbFolder, thumbName);
            using (var thumbStream = new FileStream(thumbPath, FileMode.Create))
            {
                await model.ThumbnailFile.CopyToAsync(thumbStream);
            }
            demo.ThumbnailUrl = thumbName;

            _context.DemoLessons.Add(demo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var demo = await _context.DemoLessons.FindAsync(id);
            if (demo == null) return NotFound();

            var vm = new DemoLessonEditVM
            {
                Id = demo.Id,
                Title = demo.Title,
                Description = demo.Description,
                Duration = demo.Duration,
                CourseId = demo.CourseId,
                ExistingVideoUrl = demo.VideoUrl,
                ExistingThumbnailUrl = demo.ThumbnailUrl,
                Courses = await _context.Courses.Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DemoLessonEditVM model)
        {
            var demo = await _context.DemoLessons.FindAsync(model.Id);
            if (demo == null) return NotFound();

            demo.Title = model.Title;
            demo.Description = model.Description;
            demo.Duration = model.Duration;
            demo.CourseId = model.CourseId;

            if (model.VideoFile != null)
            {
                string vidFolder = Path.Combine(_env.WebRootPath, "uploads/demovideos");
                if (!Directory.Exists(vidFolder)) Directory.CreateDirectory(vidFolder);

                if (!string.IsNullOrEmpty(demo.VideoUrl))
                {
                    string oldPath = Path.Combine(vidFolder, demo.VideoUrl);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                string vidName = Guid.NewGuid() + "_" + model.VideoFile.FileName;
                using var vidStream = new FileStream(Path.Combine(vidFolder, vidName), FileMode.Create);
                await model.VideoFile.CopyToAsync(vidStream);
                demo.VideoUrl = vidName;
            }

            if (model.ThumbnailFile != null)
            {
                string thumbFolder = Path.Combine(_env.WebRootPath, "uploads/demothumbnails");
                if (!Directory.Exists(thumbFolder)) Directory.CreateDirectory(thumbFolder);

                if (!string.IsNullOrEmpty(demo.ThumbnailUrl))
                {
                    string oldPath = Path.Combine(thumbFolder, demo.ThumbnailUrl);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                string thumbName = Guid.NewGuid() + "_" + model.ThumbnailFile.FileName;
                using var thumbStream = new FileStream(Path.Combine(thumbFolder, thumbName), FileMode.Create);
                await model.ThumbnailFile.CopyToAsync(thumbStream);
                demo.ThumbnailUrl = thumbName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var demo = await _context.DemoLessons.FindAsync(id);
            if (demo != null)
            {
                string vidPath = Path.Combine(_env.WebRootPath, "uploads/demovideos", demo.VideoUrl ?? "");
                string thumbPath = Path.Combine(_env.WebRootPath, "uploads/demothumbnails", demo.ThumbnailUrl ?? "");

                if (System.IO.File.Exists(vidPath)) System.IO.File.Delete(vidPath);
                if (System.IO.File.Exists(thumbPath)) System.IO.File.Delete(thumbPath);

                _context.DemoLessons.Remove(demo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
