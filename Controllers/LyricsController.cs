using Microsoft.AspNetCore.Mvc;
using LyricsWeb.Model;
using LyricsWeb.Service;

namespace LyricsWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LyricsController : ControllerBase
    {
        private readonly ILyricsCoverService _lyricsCoverService;
        public LyricsController(ILyricsCoverService lyricsCoverService)
        {
            _lyricsCoverService = lyricsCoverService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetLyricsAsync(
            [FromQuery] string? title,
            [FromQuery] string? album,
            [FromQuery] string? artist 
        )
        {

            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(album) && string.IsNullOrWhiteSpace(artist))
            {
                return BadRequest("parameter is required.");
            }
            string result = await _lyricsCoverService.GetLyrics(new SongInfo
            {
                Title = title,
                Album = album,
                Artist = artist
            });
            return Ok(result);
        }
    }
}
