using System;
using System.IO;
using Xunit;
using YouTube_Playlist_Tracker.Lib.YouTube;

namespace YouTube_Playlist_Tracker.Lib.Tests.YouTube
{
    public class PlaylistDataShould
    {
        [Fact]
        public void GetValidPlaylistFromURL()
        {
            // Arrange
            string url = "https://www.youtube.com/playlist?list=PLWFKnf1pcvUtSPv6-vhmaz21h_0FqpZbP";
            PlaylistData playlistData = new PlaylistData(url);

            // Assert
            Assert.NotNull(playlistData.GetFromYoutube(url));
        }

        [Fact]
        public void NotAllowNullPlaylistFromBadURL()
        {
            // Arrange
            string url = "https://www.youtube.com/playlist?list=PLWFKnf1pcvUtSPv6-vhmaz21h_0FqpZbPP";
            PlaylistData playlistData = new PlaylistData(url);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(() => playlistData.GetFromYoutube(url));
            Assert.Equal("Can't get videos in playlist, playlistConfig is null\r\nParameter name: playlistConfig", ex.Message);
        }
    }
}