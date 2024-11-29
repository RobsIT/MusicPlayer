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
    public class EconomicsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EconomicsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        //public int SongId { get; set; }
        public List<Song> AllSongsList { get; set; }
        public List<SongEconomy> SongsEconomiesList { get; set; }

        public void OnGet()
        {

            AllSongsList = _context.AllSongs.ToList();
            SongsEconomiesList = _context.SongsEconomies.ToList();
        }

        public async Task<IActionResult> OnPostAsync(int songId)
        {
            SongEconomy songEconomy = new SongEconomy();
            songEconomy.SongId = songId;
            songEconomy.SongClicks = 0;
            songEconomy.SongPlayPrice = 0.5;
            songEconomy.SongMoneyMade = 0;

            _context.SongsEconomies.Add(songEconomy);

            await _context.SaveChangesAsync();   

            return RedirectToPage();
        }
    }
}
