using LyricsWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace LyricsWeb.Data.Service
{
    public interface IDataService
    {
        public Task<List<SongItem>> GetItems(SongItem item);

        public Task<SongItem?> GetItem(SongItem item);

        public Task<SongItem?> GetCoverItem(SongItem item);

        public Task AddItem(SongItem item);

        public Task UpdateItem(SongItem item);

        public Task UpdateItemById(int id);

        public Task DeleteItem(SongItem item);

        public Task DeleteItemById(int id);
    }
}
