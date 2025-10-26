using LyricsWeb.Model;

namespace LyricsWeb.Service
{
    public interface ILyricsCoverService
    {
        Task<string> GetLyrics(SongInfo songInfo, CancellationToken cancellationToken = default);
        Task<byte[]> GetCover(SongInfo songInfo, CancellationToken cancellationToken = default);
    }
}
