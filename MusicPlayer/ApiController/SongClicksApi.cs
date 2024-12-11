using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Service;

namespace MusicPlayer.ApiController
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongClicksApiController : ControllerBase
    {
        private readonly ISongClicks _songClicks;

        public SongClicksApiController(ISongClicks songClicks)
        {
            _songClicks = songClicks;
        }

        [HttpPost("increment")]
        public async Task<IActionResult> IncrementSongClicks([FromBody] int songId)
        {
            await _songClicks.IncrementSongClicksAsync(songId);

            return Ok();
        }
    }
}
