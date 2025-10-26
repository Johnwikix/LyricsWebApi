using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LyricsWeb.Model;
using LyricsWeb.Service;

namespace LyricsWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoverController : ControllerBase
    {
        private readonly ILyricsCoverService _lyricsCoverService;
        public CoverController(ILyricsCoverService lyricsCoverService)
        {
            _lyricsCoverService = lyricsCoverService;
        }
        [HttpGet]
        public async Task<ActionResult> GetLyrics(
            [FromQuery] string? title,
            [FromQuery] string? album,
            [FromQuery] string? artist
        )
        {

            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(album) && string.IsNullOrWhiteSpace(artist))
            {
                return BadRequest("parameter is required.");
            }
            byte[] result =await _lyricsCoverService.GetCover(new SongInfo
            {
                Title = title,
                Album = album,
                Artist = artist
            });
            const string mimeType = "image/jpeg";
            return File(result,mimeType);
        }
    }
}
