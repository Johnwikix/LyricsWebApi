using System.Threading;
using LyricsWeb.APIs.CloudMusicAPI;
using LyricsWeb.Data.Service;
using LyricsWeb.Model;

namespace LyricsWeb.Service
{
    public class LyricsCoverService : ILyricsCoverService
    {
        private readonly IDataService _dataService;
        public LyricsCoverService(IDataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<byte[]> GetCover(SongInfo songInfo, CancellationToken cancellationToken = default)
        {
            return await CloudMusicSearchHelper.GetSongAlbum(songInfo.Title, songInfo.Album, songInfo.Artist);
        }

        public async Task<string> GetLyrics(SongInfo songInfo, CancellationToken cancellationToken = default)
        {
            var songItem = new SongItem
            {
                Title = songInfo.Title,
                Album = songInfo.Album,
                Artist = songInfo.Artist
            };
            var localLyrics = await _dataService.GetItem(songItem);
            if (localLyrics != null)
            {
                if (string.IsNullOrWhiteSpace(localLyrics.Lyrics)) {
                    var res = await CloudMusicSearchHelper.GetSongLyrics(songInfo.Title, songInfo.Album, songInfo.Artist, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        localLyrics.Lyrics = res;
                        await _dataService.UpdateItem(localLyrics);
                    }
                }
                return localLyrics.Lyrics;
            }
            else {
                var res = await CloudMusicSearchHelper.GetSongLyrics(songInfo.Title, songInfo.Album, songInfo.Artist, cancellationToken);
                if (!string.IsNullOrWhiteSpace(res)) {
                    songItem.Lyrics = res;
                    await _dataService.AddItem(songItem);
                }
                return res;
            }            
        }
    }
}
