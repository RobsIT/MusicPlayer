using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Moq;
using MusicPlayer.Data;
using MusicPlayer.Pages;

namespace MusicPlayerUnitTests
{
    public class FilePathTesting
    {
        [Fact]
        public void FilesCount() //Kollar antal filer
        {
            // Arrange
            int expected = 12;

            // Act
            var audioFilesList = IndexModel.GetAudioFiles("wwwroot/audio");
            int actual = audioFilesList.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnlyAcceptMp3andWavFiles()
        {
            //Arrange

            //Act
            var AudioFilesList = IndexModel.GetAudioFiles("wwwroot/audio");

            //Assert
            Assert.NotNull(AudioFilesList);
            foreach (var AudioFile in AudioFilesList)
            {
                Assert.True(AudioFile.EndsWith(".mp3") || AudioFile.EndsWith(".wav"));
            }
        }

        [Fact]
        public void DoNotPlayFilesWithMp3OrWavInNameOnly() // Should not play files that only have mp3 or wav in the name, only if its mp3 or wav audio file.
        {
            //Arrange

            //Act
            var AudioFilesList = IndexModel.GetAudioFiles("wwwroot/audio");

            //Assert
            Assert.NotNull(AudioFilesList);
            foreach (var AudioFile in AudioFilesList)
            {
                Assert.True(AudioFile.Contains("mp3") || AudioFile.Contains("wav"));
            }
        }

        [Fact]
        public void EmptyFolderReturnsEmptyList()
        {
            // Arrange
            string testPath = "wwwroot/emptyAudio";

            // Act
            var audioFilesList = IndexModel.GetAudioFiles(testPath);

            // Assert
            Assert.Empty(audioFilesList);
        }
        
        [Fact]
        public void VerifyResultHasCorrectPrefix()
        {
            // Arrange

            // Act
            var audioFilesList = IndexModel.GetAudioFiles("wwwroot/audio");

            // Assert
            foreach (var audioFile in audioFilesList)
            {
                Assert.StartsWith("/audio/", audioFile);
            }
        }
    }
}
