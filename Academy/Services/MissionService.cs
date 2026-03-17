using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.AboutUs;
using Academy.ViewModels.Mission;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class MissionService : IMissionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public MissionService(AppDbContext context,IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IEnumerable<MissionVM>> GetAllAsync()
        {
           var data = await _context.Mission.ToListAsync();
          return _mapper.Map<IEnumerable<MissionVM>>(data);


        }

        public  async Task CreateAsync(MissionCreateVM mission)
        {
           string folderPath = Path.Combine(_env.WebRootPath, "uploads", "mission");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = $"{Guid.NewGuid()}_{mission.Image.FileName}";
            string path = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(path , FileMode.Create))
            {
                await mission.Image.CopyToAsync(stream);
            }
            var missionData = _mapper.Map<Mission>(mission);
            missionData.Image = fileName;
            missionData.CreatedAt = DateTime.Now;
             await _context.Mission.AddAsync(missionData);
             await _context.SaveChangesAsync();
        }

        public async  Task<Mission> GetByIdAsync(int id)
        {
           var data = await _context.Mission.FindAsync(id);
            return data;
        }

        public  async Task DeleteAsync(Mission mission)
        {
            var data = await _context.Mission.FindAsync(mission.Id);
            var path = Path.Combine(_env.WebRootPath, "uploads", "mission", data.Image);
                if (File.Exists(path))
                {
                    File.Delete(path);
            } 
                _context.Mission.Remove(data);
                await _context.SaveChangesAsync();

        }
    }
}
