using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Models;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using MusicPlayer.Service;

namespace MusicPlayerUnitTests
{
    public class SongClicksTest
    {

        //Testing the SongClicks Service, if song exist increase with 1, if song does not exist return 0
        [Theory]
        [InlineData(1, 1)]
        [InlineData(99, 0)]
        public async Task IncrementSongClicksUnitTesting(int requestSongId, int expectedClicks)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IncrementSongClicks")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var song = new SongEconomy
                {
                    SongId = 1,
                    SongClicks = 0
                };

                context.SongsEconomies.Add(song);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var songClicks = new SongClicks(context);

                // Act
                await songClicks.IncrementSongClicksAsync(requestSongId);

                // Assert
                
                var song = context.SongsEconomies.FirstOrDefault(s => s.SongId == requestSongId);

                if (song == null)
                {
                    Assert.Equal(expectedClicks, 0);
                }
                else
                {
                    Assert.Equal(expectedClicks, song.SongClicks);
                }
            }
        }
    }
}
