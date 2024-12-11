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

        [Fact] // Call the EconomicsViewModel, and calls the OnPostAsync to test the database add function
        public async Task AddSongEconomyTest()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var songEconomy = new SongEconomy
                {
                    SongId = 1,
                    SongPlayPrice = 0.5,
                    SongClicks = 0
                };

                context.SongsEconomies.Add(songEconomy);
                context.SaveChanges();
            }


            using (var context = new ApplicationDbContext(options))
            {
                // Act
                var songEconomyModel = new EconomicsModel(context);

                var result = await songEconomyModel.OnPostAsync(3);

                // Assert
                var songEconomy = context.SongsEconomies.FirstOrDefault(se => se.SongId == 3);

                Assert.NotNull(songEconomy);
                Assert.Equal(0, songEconomy.SongClicks);
                Assert.Equal(0.5, songEconomy.SongPlayPrice);

                // Check the return type
                Assert.IsType<RedirectToPageResult>(result);
            }


        }

    }
}






