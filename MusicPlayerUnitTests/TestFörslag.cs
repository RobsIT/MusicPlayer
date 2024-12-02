using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MusicPlayer.Models;
using static MusicPlayer.Pages.IndexModel;
public class SongPageModelTests
{
    private List<SongEconomy> CreateTestData()
    {
        return new List<SongEconomy>
        {
            new SongEconomy { Id = 1, SongId = 1, SongClicks = 5, SongPlayPrice = 0.5 },
            new SongEconomy { Id = 2, SongId = 2, SongClicks = 56, SongPlayPrice = 0.5 },
            new SongEconomy { Id = 3, SongId = 3, SongClicks = 12, SongPlayPrice = 0.5 },
            new SongEconomy { Id = 4, SongId = 4, SongClicks = 0, SongPlayPrice = 0.5 }
        };
    }

    [Fact]
    public async Task OnPostUpdateSongClicksAsync_Should_Return_False_For_Invalid_SongId()
    {
        // Arrange
        var testData = CreateTestData();
        var pageModel = new SongPageModel(testData);
        var invalidRequest = new SongClickRequest { SongId = 0 };

        // Act
        var result = await pageModel.OnPostUpdateSongClicksAsync(invalidRequest) as JsonResult;
        var response = result?.Value as dynamic;

        // Assert
        Assert.NotNull(response);
        Assert.False(response.success);
        Assert.Equal("Invalid song ID (must be greater than 0)", response.message);
    }

    [Fact]
    public async Task OnPostUpdateSongClicksAsync_Should_Return_False_If_SongEconomy_Not_Found()
    {
        // Arrange
        var testData = CreateTestData();
        var pageModel = new SongPageModel(testData);
        var request = new SongClickRequest { SongId = 99 };

        // Act
        var result = await pageModel.OnPostUpdateSongClicksAsync(request) as JsonResult;
        var response = result?.Value as dynamic;

        // Assert
        Assert.NotNull(response);
        Assert.False(response.success);
        Assert.Equal("No SongEconomy found for SongId 99", response.message);
    }

    [Fact]
    public async Task OnPostUpdateSongClicksAsync_Should_Increment_SongClicks()
    {
        // Arrange
        var testData = CreateTestData();
        var pageModel = new SongPageModel(testData);
        var request = new SongClickRequest { SongId = 1 };

        // Act
        var result = await pageModel.OnPostUpdateSongClicksAsync(request) as JsonResult;
        var response = result?.Value as dynamic;

        // Assert
        Assert.NotNull(response);
        Assert.True(response.success);
        Assert.Equal(6, testData.First(s => s.SongId == 1).SongClicks);
    }
}

public class SongPageModel
{
    private readonly List<SongEconomy> _songsEconomies;

    public SongPageModel(List<SongEconomy> songsEconomies)
    {
        _songsEconomies = songsEconomies;
    }

    public async Task<IActionResult> OnPostUpdateSongClicksAsync(SongClickRequest request)
    {
        if (request.SongId <= 0)
        {
            return new JsonResult(new { success = false, message = "Invalid song ID (must be greater than 0)" });
        }

        var songEconomy = _songsEconomies.FirstOrDefault(s => s.SongId == request.SongId);
        if (songEconomy == null)
        {
            return new JsonResult(new { success = false, message = $"No SongEconomy found for SongId {request.SongId}" });
        }

        songEconomy.SongClicks += 1;
        await Task.CompletedTask; // Simulate async behavior

        return new JsonResult(new { success = true });
    }
}

