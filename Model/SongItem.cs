namespace LyricsWeb.Model
{
    public class SongItem
    {
        public int Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Album { get; set; } = string.Empty;
        public string? Artist { get; set; } = string.Empty;
        public string? Lyrics { get; set; } = string.Empty;
        public string? CoverPath { get; set; } = string.Empty;
    }
}
