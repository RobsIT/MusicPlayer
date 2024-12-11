using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;

namespace MusicPlayer.Service
{
    public class SongClicks : ISongClicks
    {
        private readonly ApplicationDbContext _context;

        public SongClicks(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task IncrementSongClicksAsync(int songId)
        {
           
            var song = await _context.SongsEconomies.FirstOrDefaultAsync(s => s.SongId == songId);
            if (song != null)
            {
                song.SongClicks++;
                await _context.SaveChangesAsync();
            }
        }
    }
}
