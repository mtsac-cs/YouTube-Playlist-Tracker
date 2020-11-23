using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace YouTube_Playlist_Tracker.Lib.YouTube
{
    public class YoutubePlaylistAPI
    {
        private string APIKey;
        private string playListID;
        private const int videoListSize = 50;
        public static string apiKeyFilePath = Environment.CurrentDirectory + "\\api.txt";

        internal YoutubePlaylistAPI(string playListURL = "")
        {
            APIKey = GetAPIKeyFromFile();
            SetPlaylistID(playListURL);
        }

        public static bool DoesApiFileExist()
        {
            return File.Exists(apiKeyFilePath);
        }

        private string GetAPIKeyFromFile()
        {
            if (!File.Exists(apiKeyFilePath))
                throw new Exception("Can't get API key from file. api.txt file does not exist. Current path: " + apiKeyFilePath);

            string text = File.ReadAllText(apiKeyFilePath);
            if (String.IsNullOrEmpty(text))
                Logger.Log("Can't get API key from file. There was no text inside api.txt");

            return text;
        }

        public static void CreateAPIFile()
        {
            File.Create(apiKeyFilePath);
        }

        internal void SetPlaylistID(string url)
        {
            playListID = GetPlaylistIDFromURL(url);
        }

        private string GetPlaylistIDFromURL(string url)
        {
            //PlayList ID is everything after the equal sign
            string[] split = url.Split('=');
            string playlistId = split[split.Length - 1];
            
            const int playlistIDLength = 34;
            if (playlistId.Length != playlistIDLength)
                throw new Exception("Playlist ID is not the correct length. It's must be 34 char long. Length: " + playListID.Length);

            return playlistId;
        }


        /// <summary>
        /// Create PlaylistInfo by using a playlist's url and YouTube API
        /// </summary>
        /// <returns></returns>
        internal PlaylistInfo GetPlaylistInfo()
        {
            var playlistConfig = CreatePlaylistConfig();
            if (playlistConfig is null)
                return null;

            var videos = GetVideosInPlaylist(playlistConfig);

            PlaylistInfo playlistInfo = new PlaylistInfo();
            playlistInfo.PlaylistVideos = videos;
            playlistInfo.playlistID = playlistConfig.Items[0].Snippet.PlaylistId;
            return playlistInfo;
        }

        private List<VideoInfo> GetVideosInPlaylist(YoutubePlaylistConfig playlistConfig)
        {
            Guard.ThrowIfArgumentIsNull(playlistConfig, "Can't get videos in playlist, playlistConfig is null", "playlistConfig");

            List<VideoInfo> videos = new List<VideoInfo>();
            foreach (var item in playlistConfig.Items)
            {
                VideoInfo video = new VideoInfo();
                video.Title = item.Snippet.Title;
                video.IndexInPlaylist = (int)item.Snippet.Position;
                video.Description = "not implemented yet";
                videos.Add(video);
            }

            return videos;
        }

        private YoutubePlaylistConfig CreatePlaylistConfig()
        {
            string json = GetPlaylistJSONFromID();
            if (String.IsNullOrEmpty(json))
                return null;

            return YoutubePlaylistConfig.FromJson(json);
        }

        private string GetPlaylistJSONFromID()
        {
            if (!CanGetPlaylistFromJSON())
                return null;

            var parameters = new Dictionary<string, string>
            {
                //Store API Key
                ["key"] = APIKey,
                //Store the Playlisy ID
                ["playlistId"] = playListID,
                //Get only the info in this part
                ["part"] = "snippet",
                //get on the info in this feild for this part
                //["fields"] = "pageInfo, items/snippet(title)", //removed this to get more info. Leaving for reference
                //Max number of video you can pull
                ["maxResults"] = videoListSize.ToString()
            };

            //Create full URL with API key
            var fullUrl = MakeUrlWithQuery(parameters);

            WebHandler webHandler = new WebHandler();
            string json = webHandler.ReadText_FromURL(fullUrl);
            return json;
        }

        private bool CanGetPlaylistFromJSON()
        {
            if (string.IsNullOrEmpty(APIKey))
            {
                Logger.Log("You need to put your api key in \"api.txt\", located in the same folder as this program");
                return false;
            }

            if (String.IsNullOrEmpty(playListID))
            {
                Logger.Log("This program needs the playlistID in order to get playlist from YT");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Use parameters to get playlist url. Needed to apply API key and sort through YT's video hierarchy
        /// </summary>
        private string MakeUrlWithQuery(Dictionary<string, string> parameters)
        //private string MakeUrlWithQuery(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var baseUrl = "https://www.googleapis.com/youtube/v3/playlistItems?";

            //Empty string case
            if (parameters == null || parameters.Count() == 0)
                return baseUrl;

            //Return the full Url
            return parameters.Aggregate(baseUrl,
                (accumulated, kvp) => string.Format($"{accumulated}{kvp.Key}={kvp.Value}&"));
        }
    }
}