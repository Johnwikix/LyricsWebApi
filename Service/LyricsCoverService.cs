using System.Threading;
using LyricsWeb.APIs.CloudMusicAPI;
using LyricsWeb.Model;

namespace LyricsWeb.Service
{
    public class LyricsCoverService : ILyricsCoverService
    {
        public async Task<byte[]> GetCover(SongInfo songInfo, CancellationToken cancellationToken = default)
        {
            return await CloudMusicSearchHelper.GetSongAlbum(songInfo.Title, songInfo.Album, songInfo.Artist);
        }

        public async Task<string> GetLyrics(SongInfo songInfo, CancellationToken cancellationToken = default)
        {
            return await CloudMusicSearchHelper.GetSongLyrics(songInfo.Title, songInfo.Album, songInfo.Artist, cancellationToken);
        }
    }
}
