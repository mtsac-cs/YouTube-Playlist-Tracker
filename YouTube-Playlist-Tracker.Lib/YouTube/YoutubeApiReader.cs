using System.Collections.Generic;

namespace YouTube_Playlist_Tracker.Lib.YouTube
{
    /// <summary>
    /// This class interacts with PlaylistAPI to get usable info from it
    /// </summary>
    public class YoutubeApiReader
    {
        readonly string url;
        public YoutubeApiReader(string url)
        {
            this.url = url;
        }

        internal PlaylistConfig GetPlaylistConfig()
        {
            YoutubeApi api = new YoutubeApi(url);
            string json = api.GetJsonFromYouTube();
            return PlaylistConfig.FromJson(json);
        }

        internal List<VideoData> GetVideosInPlaylist(PlaylistConfig playlistConfig)
        {
            Guard.ThrowIfArgumentIsNull(playlistConfig, "playlistConfig", "Can't get videos in playlist, playlistConfig is null");

            List<VideoData> videos = new List<VideoData>();
            foreach (var item in playlistConfig.Items)
            {
                VideoData video = new VideoData();
                video.Title = item.Snippet.Title;
                video.IndexInPlaylist = (int)item.Snippet.Position;
                video.Description = "not implemented yet";
                videos.Add(video);
            }

            return videos;
        }
    }
}
