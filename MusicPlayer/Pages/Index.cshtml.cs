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
using MusicPlayer.Service;

namespace MusicPlayer.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISongClicks _songClicks;
        public IndexModel(ApplicationDbContext context, ISongClicks songClicks)
        {
            _context = context;
            _songClicks = songClicks;
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
            AudioFilesList = GetAudioFiles("wwwroot/audio");
            SongsEconomiesList = _context.SongsEconomies.ToList();
            PlaylistId = playlistId;
            SongId = songId;
            AllSongsList = _context.AllSongs.ToList();
            PlaylistList = _context.Playlists.ToList();
            SongsAddedToPlaylistsList = _context.SongsAddedToPlaylists.ToList();
        }

        public static List<string> GetAudioFiles(string path)
        {
            var audioDirectory = Path.Combine(path);
            if (!Directory.Exists(audioDirectory))
            {
                return new List<string>();
            }
            return Directory.GetFiles(audioDirectory)
                                    .Where(file => file.EndsWith(".mp3") || 
                                                   file.EndsWith(".wav"))
                                    .Select(file => "/audio/" + Path.GetFileName(file))
                                    .ToList();
        }

        public async Task<IActionResult> OnPostAsync(int songId, int playlistId)
        {
            await _songClicks.IncrementSongClicksAsync(songId);

            string audiofileName = AudioFileName;
            Song audioFileNameObj = new Song(); 
            audioFileNameObj.SongFileName = audiofileName;

            if (audioFileNameObj.SongFileName != null)
            {
                _context.AllSongs.Add(audioFileNameObj); // Adds to DbSet
            }

            if (PlaylistObj.PlaylistName != null)
            {
                _context.Playlists.Add(PlaylistObj); // Adds to DbSet
            }

            if ((songId != null || playlistId != null) || (songId != 0 || playlistId != 0))
            {
                SongAddedToPlaylist addedToPlaylist = new SongAddedToPlaylist();
                addedToPlaylist.SongFileId = songId;
                addedToPlaylist.PlaylistId = playlistId;

                _context.SongsAddedToPlaylists.Add(addedToPlaylist); // Adds to DbSet
            }

            await _context.SaveChangesAsync();   // Save changes to Db

            return RedirectToPage();
        }
    }
}
