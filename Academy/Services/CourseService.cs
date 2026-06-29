using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Course;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public CourseService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IEnumerable<CourseVM>> GetAllAsync()
        {
            var data = await _context.Courses
                .Include(x => x.Language)
                .Include(x => x.Category)
                .Include(x => x.Instructor)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CourseVM>>(data);
        }

        public async Task CreateAsync(CourseCreateVM model)
        {
            string folder = Path.Combine(_env.WebRootPath, "uploads/course");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fileName = null;

            if (model.Image != null)
            {
                Academy.Helpers.FileHelper.ValidateImage(model.Image);
                fileName = await Academy.Helpers.FileHelper.SaveFileAsync(model.Image, folder);
            }

            var data = _mapper.Map<Course>(model);
            data.ImageUrl = fileName;
            data.CreatedDate = DateTime.Now;
            // CourseName-dən Title-ı avtomatik doldur
            if (model.CourseNameId > 0)
            {
                var courseName = await _context.CourseNames.FindAsync(model.CourseNameId);
                if (courseName != null && string.IsNullOrWhiteSpace(model.Title))
                    data.Title = courseName.Name;
                data.CourseNameId = model.CourseNameId;
            }

            if (model.VideoTitles != null && model.VideoTitles.Count > 0)
            {
                data.Videos = new List<Video>();
                string vidFolder = Path.Combine(_env.WebRootPath, "uploads/videos");
                if (!Directory.Exists(vidFolder)) Directory.CreateDirectory(vidFolder);

                for (int i = 0; i < model.VideoTitles.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(model.VideoTitles[i]))
                    {
                        string vidName = null;
                        if (model.VideoFiles != null && model.VideoFiles.Count > i && model.VideoFiles[i] != null)
                        {
                            Academy.Helpers.FileHelper.ValidateVideo(model.VideoFiles[i]);
                            vidName = await Academy.Helpers.FileHelper.SaveFileAsync(model.VideoFiles[i], vidFolder);
                        }

                        data.Videos.Add(new Video
                        {
                            Title = model.VideoTitles[i],
                            Url = vidName,
                            Level = model.VideoLevels != null && model.VideoLevels.Count > i
                                ? model.VideoLevels[i]
                                : Academy.Models.VideoLevel.Beginner
                        });
                    }
                }
            }

            await _context.Courses.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<CourseDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Courses
     .Include(x => x.Language)
     .Include(x => x.Category)
     .Include(x => x.Instructor)
     .Include(x => x.Features)
      .Include(x => x.Requirements)
      .Include(x => x.Lessons)
      .Include(x => x.Videos)
      .Include(x => x.DemoLessons)
     .FirstOrDefaultAsync(x => x.Id == id);

            var vm = _mapper.Map<CourseDetailVM>(data);
            if (data != null)
            {
                if (data.Videos != null) vm.Videos = data.Videos.ToList();
                if (data.DemoLessons != null) vm.DemoLessons = data.DemoLessons.ToList();
                
                // Fetch scheduled Live Classes for this course
                var liveClasses = await _context.LiveClasses.Where(l => l.CourseId == id && l.Status != LiveSessionStatus.Ended && l.Status != LiveSessionStatus.Canceled).ToListAsync();
                vm.LiveClasses = liveClasses;
            }
            return vm;
        }

     
        public async Task DeleteAsync(int id)
        {
            var data = await _context.Courses.FindAsync(id);
            if (data == null) return;

            _context.Courses.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseEditVM model)
        {
            var data = await _context.Courses
                .Include(x => x.Videos)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (data == null) return;

           
            if (model.ImageFile != null)
            {
                Academy.Helpers.FileHelper.ValidateImage(model.ImageFile);
                string folder = Path.Combine(_env.WebRootPath, "uploads/course");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                Academy.Helpers.FileHelper.DeleteFile(folder, data.ImageUrl);
                data.ImageUrl = await Academy.Helpers.FileHelper.SaveFileAsync(model.ImageFile, folder);
            }

           
            data.Title = model.Title;
            data.Description = model.Description;
            data.Price = model.Price;
            data.Duration = model.Duration;
            data.StudentCount = model.StudentCount;
            data.Level = model.Level;

            data.LanguageId = model.LanguageId;
            data.CategoryId = model.CategoryId;
            data.InstructorId = model.InstructorId;

            if (model.VideoTitles != null && model.VideoTitles.Count > 0)
            {
                if (data.Videos == null) data.Videos = new List<Video>();
                for (int i = 0; i < model.VideoTitles.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(model.VideoTitles[i]))
                    {
                        string vidName = null;
                        if (model.VideoFiles != null && model.VideoFiles.Count > i && model.VideoFiles[i] != null)
                        {
                            string vidFolder = Path.Combine(_env.WebRootPath, "uploads/videos");
                            if (!Directory.Exists(vidFolder)) Directory.CreateDirectory(vidFolder);
                            vidName = Guid.NewGuid() + "_" + model.VideoFiles[i].FileName;
                            string vidPath = Path.Combine(vidFolder, vidName);
                            using var vidStream = new FileStream(vidPath, FileMode.Create);
                            await model.VideoFiles[i].CopyToAsync(vidStream);
                        }

                        data.Videos.Add(new Video
                        {
                            Title = model.VideoTitles[i],
                            Url = vidName,
                            Level = model.VideoLevels != null && model.VideoLevels.Count > i ? model.VideoLevels[i] : Academy.Models.VideoLevel.Beginner
                        });
                    }
                }
            }

            data.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
