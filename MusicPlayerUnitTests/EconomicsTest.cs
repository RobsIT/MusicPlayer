using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Models;
using static MusicPlayer.Pages.IndexModel;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using MusicPlayer.Pages;

namespace MusicPlayerUnitTests
{

        public class EconomicsTest
        {
            private readonly ApplicationDbContext _context;

            public EconomicsTest()
            {
                // Use in-memory database för att skapa en testDatabase att använda
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;

                _context = new ApplicationDbContext(options);

                _context.Database.EnsureDeleted();

                // Lägger till låtar och en låt till songEconomy
                _context.AllSongs.AddRange(new List<Song>
            {
                new Song { Id = 4, SongFileName = "Song1.mp3" },
                new Song { Id = 2, SongFileName = "Song2.mp3" }
            });

                _context.SongsEconomies.AddRange(new List<SongEconomy>
            {
                new SongEconomy { Id = 1, SongId = 4, SongClicks = 10, SongPlayPrice = 0.5 }
            });

                _context.SaveChanges();
            }

            [Fact]
            public void OnGet_PopulatesModelProperties()
            {
                // Arrange
                var model = new EconomicsModel(_context);

                // Act
                model.OnGet();

                // Assert
                Assert.Equal(2, model.AllSongsList.Count);
                Assert.Equal(1, model.SongsEconomiesList.Count);
            }

            [Fact]
            public async Task OnPostAsync_AddsNewSongEconomy()
            {
                // Arrange
                var model = new EconomicsModel(_context);

                // Act
                var result = await model.OnPostAsync(3); // Lägger till en ny SongEconomy och kollar att den har standard values

                // Assert
                var songEconomy = _context.SongsEconomies.FirstOrDefault(se => se.SongId == 3);
                Assert.NotNull(songEconomy);
                Assert.Equal(0, songEconomy.SongClicks);
                Assert.Equal(0.5, songEconomy.SongPlayPrice);

                // Check the return type
                Assert.IsType<RedirectToPageResult>(result);
            }

        }


}






