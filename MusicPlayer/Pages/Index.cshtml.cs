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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

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
        public void OnGet(int songId, int playlistId)
        {

            
            // Hämta alla ljudfiler från wwwroot/audio
            var audioDirectory = Path.Combine("wwwroot/audio");
            if (Directory.Exists(audioDirectory))
            {
                AudioFilesList = Directory.GetFiles(audioDirectory)
                                          .Select(file => "/audio/" + Path.GetFileName(file))
                                          .ToList();
            }
            PlaylistId = playlistId;
            SongId = songId;
            AllSongsList = _context.AllSongs.ToList(); // Hämta alla poster AllSongs
            PlaylistList = _context.Playlists.ToList(); // Hämta alla poster från Playlist-tabellen
            SongsAddedToPlaylistsList = _context.SongsAddedToPlaylists.ToList(); // Hämta alla poster från SongsAddedToPlaylists-tabellen
        }
       
        public async Task<IActionResult> OnPostAsync(int songId, int playlistId)
        {
            int songId02 = songId;
            int playlistId02 = playlistId;


            string audiofileName = AudioFileName;
            Song audioFileNameObj = new Song();
            audioFileNameObj.SongFileName = audiofileName;
            
            if(audioFileNameObj.SongFileName != null) 
            {
                _context.AllSongs.Add(audioFileNameObj); // Lägger till i DbSet
            }
            
            if (PlaylistObj.PlaylistName != null) 
            {
                _context.Playlists.Add(PlaylistObj); // Lägger till i DbSet
            }
            
          
            if((songId != null || playlistId != null) || (songId != 0 || playlistId != 0)) 
            {
                SongAddedToPlaylist addedToPlaylist = new SongAddedToPlaylist();
                addedToPlaylist.SongFileId = songId;
                addedToPlaylist.PlaylistId = playlistId;
                
                _context.SongsAddedToPlaylists.Add(addedToPlaylist); // Lägger till i DbSet
            }

            
            
            await _context.SaveChangesAsync();   // Sparar ändringarna i databasen


            return RedirectToPage();
        }
    }
}

//public async Task<IActionResult> OnPostAsync()
//{
//    if (AudioFiles != null && AudioFiles.Any())
//    {
//        foreach (var audioFile in AudioFiles)
//        {
//            var filePath = Path.Combine("wwwroot/audio", audioFile.FileName);

//            // Kontrollera om filen redan finns för att undvika duplicering
//            if (!System.IO.File.Exists(filePath))
//            {
//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await audioFile.CopyToAsync(stream);
//                }
//            }
//        }
//    }

//    _context.Playlists.Add(PlaylistObj); // Lägger till i DbSet
//    await _context.SaveChangesAsync();   // Sparar ändringarna i databasen


//    return RedirectToPage();
//}