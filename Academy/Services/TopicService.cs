using Academy.Data;
using Academy.Models;
using Academy.Services.Interfaces;
using Academy.ViewModels.Topic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academy.Services
{
    public class TopicService : ITopicService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TopicService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(TopicCreateVM model)
        {
            var data = _mapper.Map<Topic>(model);
            await _context.Topics.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Topics
                .Include(x => x.Articles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return;

            if (data.Articles != null && data.Articles.Any())
            {
                _context.Articles.RemoveRange(data.Articles);
            }

            _context.Topics.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TopicVM>> GetAllAsync()
        {
            var data = await _context.Topics
                .Include(x => x.Articles)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TopicVM>>(data);
        }

        public async Task<TopicDetailVM> GetByIdAsync(int id)
        {
            var data = await _context.Topics
                .Include(x => x.Articles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return null;

            return _mapper.Map<TopicDetailVM>(data);
        }

        public async Task UpdateAsync(Topic topic)
        {
            _context.Topics.Update(topic);
            await _context.SaveChangesAsync();
        }
    }
}
