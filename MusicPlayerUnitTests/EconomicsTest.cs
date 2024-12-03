using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Models;
using static MusicPlayer.Pages.IndexModel;

namespace MusicPlayerUnitTests
{
    public class EconomicsTest
    {

        [Fact]
        public async Task TestingEconomicsGetMethod() //ENHETSTEST
        {
            // Arrange
          

            // Act
      

            // Assert
        
        }
        
        [Fact]
        public async Task TestingEconomicsPostMethod() //ENHETSTEST
        {
            // Arrange
          

            // Act
      

            // Assert
        
        }
        
        [Fact]
        public async Task TestingEconomicsClicksMethod() //ENHETSTEST
        {
            // Arrange
          

            // Act
      

            // Assert
        
        }
   
    
    }
    

    //--------------------------KOMMENTERA UT VID KÖRNING--------------------------------------
    //[Fact]
    //public async Task OnPostUpdateSongClicksAsync_Should_Return_False_For_Invalid_SongId()
    //{
    //    // Arrange
    //    var testData = CreateTestData();
    //    var pageModel = new SongPageModel(testData);
    //    var invalidRequest = new SongClickRequest { SongId = 0 };

    //    // Act
    //    var result = await pageModel.OnPostUpdateSongClicksAsync(invalidRequest) as JsonResult;
    //    var response = result?.Value as dynamic;

    //    // Assert
    //    Assert.NotNull(response);
    //    Assert.False(response.success);
    //    Assert.Equal("Invalid song ID (must be greater than 0)", response.message);
    //}


}

