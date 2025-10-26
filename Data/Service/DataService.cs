using LyricsWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace LyricsWeb.Data.Service
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _context;

        public DataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SongItem>> GetItems(SongItem item)
        {
            return _context.SongItems.Where(i => (string.IsNullOrEmpty(item.Title) || i.Title == item.Title) &&
                            (string.IsNullOrEmpty(item.Album) || i.Album == item.Album) &&
                            (string.IsNullOrEmpty(item.Artist) || i.Artist == item.Artist))
                .ToList();
        }

        public async Task<SongItem?> GetItem(SongItem item)
        {
            return _context.SongItems.FirstOrDefault(i => (string.IsNullOrEmpty(item.Title) || i.Title == item.Title) &&
                            (string.IsNullOrEmpty(item.Album) || i.Album == item.Album) &&
                            (string.IsNullOrEmpty(item.Artist) || i.Artist == item.Artist));
        }

        public async Task AddItem(SongItem item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItem(SongItem item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemById(int id)
        {
            var item = _context.SongItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _context.Update(item);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(SongItem item)
        {
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemById(int id)
        {
            var item = _context.SongItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _context.Remove(item);
            }
            await _context.SaveChangesAsync();
        }
    }
}
