using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Slider;
using AutoMapper;

namespace Academy.Services
{
    public class SliderService : ISliderService
    {
      
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;



        public SliderService(AppDbContext context,IWebHostEnvironment env,IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async  Task CreateAsync(SliderCreateVM slider)
        {
            string folderPath = Path.Combine(_env.WebRootPath,"uploads","sliders");
            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = $"{Guid.NewGuid()}_{slider.Image.FileName}";
            string path = Path.Combine(folderPath,fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await slider.Image.CopyToAsync(stream);
            }

           var sliders = _mapper.Map<Slider>(slider);
            sliders.Image = fileName;
            sliders.CreatedAt = DateTime.Now;
            await _context.Sliders.AddAsync(sliders);
            await _context.SaveChangesAsync();
        }
    }
}
