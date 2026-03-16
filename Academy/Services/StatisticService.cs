using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Statistic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class StatisticService : IStatisticService
    {
      private readonly AppDbContext _context;
      private readonly IMapper _mapper;
     private IWebHostEnvironment _env;
        public StatisticService(AppDbContext context,IMapper mapper,IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }



        public async Task CreateAsync(StatisticCreateVM model)
        {
            var statistic = _mapper.Map<Statistic>(model);
            statistic.CreatedAt = DateTime.Now;
            await _context.Statistics.AddAsync(statistic);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Statistic statistic)
        {
            var data  = await _context.Statistics.FindAsync(statistic.Id);
           _context.Statistics.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StatisticVM>> GetAllAsync()
        {
            var data = await _context.Statistics.ToListAsync();
            return _mapper.Map<IEnumerable<StatisticVM>>(data);
        }

        public async Task<Statistic> GetByIdAsync(int id)
        {
           var data = await _context.Statistics.FindAsync(id);
            return data;
        }
    }
}
