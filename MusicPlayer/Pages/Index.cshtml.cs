using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using MusicPlayer.Models;
using MusicPlayer.Data;

namespace MusicPlayer.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public class SongClickRequest
        {
            public int SongId { get; set; }
        }
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<IFormFile> AudioFiles { get; set; }
        public List<string> AudioFilesList { get; set; } = new List<string>();

        [BindProperty]
        public string AudioFileName { get; set; }

        [BindProperty]
        public Playlist PlaylistObj { get; set; }
        public List<Playlist> PlaylistList { get; set; }
        public List<Song> AllSongsList { get; set; }
        public List<SongAddedToPlaylist> SongsAddedToPlaylistsList { get; set; }
        public int SongId { get; set; }
        public int PlaylistId { get; set; }
        public List<SongEconomy> SongsEconomiesList { get; set; }
        public void OnGet(int songId, int playlistId)
        {

            var audioDirectory = Path.Combine("wwwroot/audio");
            if (Directory.Exists(audioDirectory))
            {
                AudioFilesList = Directory.GetFiles(audioDirectory)
                                          .Select(file => "/audio/" + Path.GetFileName(file))
                                          .ToList();
            }

            SongsEconomiesList = _context.SongsEconomies.ToList();
            PlaylistId = playlistId;
            SongId = songId;
            AllSongsList = _context.AllSongs.ToList();
            PlaylistList = _context.Playlists.ToList();
            SongsAddedToPlaylistsList = _context.SongsAddedToPlaylists.ToList();
        }

        public async Task<IActionResult> OnPostAsync(int songId, int playlistId)
        {

            string audiofileName = AudioFileName;
            Song audioFileNameObj = new Song();
            audioFileNameObj.SongFileName = audiofileName;

            if (audioFileNameObj.SongFileName != null)
            {
                _context.AllSongs.Add(audioFileNameObj); // Lägger till i DbSet
            }

            if (PlaylistObj.PlaylistName != null)
            {
                _context.Playlists.Add(PlaylistObj); // Lägger till i DbSet
            }

            if ((songId != null || playlistId != null) || (songId != 0 || playlistId != 0))
            {
                SongAddedToPlaylist addedToPlaylist = new SongAddedToPlaylist();
                addedToPlaylist.SongFileId = songId;
                addedToPlaylist.PlaylistId = playlistId;

                _context.SongsAddedToPlaylists.Add(addedToPlaylist); // Lägger till i DbSet
            }

            await _context.SaveChangesAsync();   // Sparar ändringarna i databasen

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateSongClicksAsync([FromBody] SongClickRequest request)
        {
            Console.WriteLine($"Received songId: {request.SongId}"); // Debug log
            if (request.SongId <= 0)
            {
                return new JsonResult(new { success = false, message = "Invalid song ID (must be greater than 0)" });
            }

            var songEconomy = await _context.SongsEconomies.FirstOrDefaultAsync(s => s.SongId == request.SongId);
            if (songEconomy == null)
            {
                return new JsonResult(new { success = false, message = $"No SongEconomy found for SongId {request.SongId}" });
            }

            songEconomy.SongClicks += 1;
            
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
