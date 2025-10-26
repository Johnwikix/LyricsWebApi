using LyricsWeb.APIs.CloudMusicAPI;
using LyricsWeb.Data.Service;
using LyricsWeb.Model;
using System.Security.Cryptography;
using System.Threading;

namespace LyricsWeb.Service
{
    public class LyricsCoverService : ILyricsCoverService
    {
        private readonly IDataService _dataService;
        private readonly IWebHostEnvironment _env;
        private readonly string _coverCachePath;
        public LyricsCoverService(IDataService dataService, IWebHostEnvironment env)
        {
            _dataService = dataService;
            _env = env;
            _coverCachePath = Path.Combine(_env.ContentRootPath, "CoverCache");
            Directory.CreateDirectory(_coverCachePath);
        }
        public async Task<byte[]> GetCover(SongInfo songInfo, CancellationToken cancellationToken = default)
        {
            var songItem = new SongItem
            {
                Title = songInfo.Title,
                Album = songInfo.Album,
                Artist = songInfo.Artist
            };

            var localItem = await _dataService.GetCoverItem(songItem);

            if (localItem != null && !string.IsNullOrEmpty(localItem.CoverPath))
            {
                var fullPath = Path.Combine(_coverCachePath, localItem.CoverPath);
                if (File.Exists(fullPath))
                {
                    return await File.ReadAllBytesAsync(fullPath, cancellationToken);
                }
            }

            var coverBytes = await CloudMusicSearchHelper.GetSongAlbum(
                songInfo.Title, songInfo.Album, songInfo.Artist);

            if (coverBytes != null && coverBytes.Length > 0)
            {
                var fileHash = GetSha256Hash(coverBytes);
                var fileName = $"{fileHash}.jpg";
                var fullPath = Path.Combine(_coverCachePath, fileName);
                await File.WriteAllBytesAsync(fullPath, coverBytes, cancellationToken);
                if (localItem == null)
                {
                    songItem.CoverPath = fileName;
                    await _dataService.AddItem(songItem);
                }
                else
                {
                    localItem.CoverPath = fileName;
                    await _dataService.UpdateItem(localItem);
                }
            }

            return coverBytes;
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

        private static string GetSha256Hash(byte[] bytes)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
