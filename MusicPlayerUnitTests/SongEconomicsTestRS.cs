using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Models;
using Xunit;

namespace MusicPlayerUnitTests
{
    public class SongEconomicsTestRS
    {
        [Fact]
        public void Metod1()
        {
            // Arrange
            int min = 1;
            int max = 20;

            // Act
            int actual = request.SongId;
            // Assert
            Assert.InRange(request, min, max );
            
        }

        [Fact]
        public void Metod2()
        {
            // Arrange
            var sut = 0;
            var expected = 4;
            // Act
            var actual = 0;
            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
