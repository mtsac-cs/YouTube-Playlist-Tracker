using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using YouTube_Playlist_Tracker.Lib.Web;

namespace YouTube_Playlist_Tracker.Lib.YouTube
{
    /// <summary>
    /// This class uses API Key to get string text about playlist
    /// Much of this code was inspired by this Youtube Tutorial: https://www.youtube.com/watch?v=gRXE8QkFqtU
    /// </summary>
    public class PlaylistAPI
    {
        private string APIKey;
        private string playListID;
        private const int videoListSize = 50;
        public static string apiKeyFilePath = Environment.CurrentDirectory + "\\api.txt";

        internal PlaylistAPI(string playListURL)
        {
            APIKey = GetAPIKeyFromFile();
            SetPlaylistID(playListURL);
        }

        public static bool DoesApiFileExist() => File.Exists(apiKeyFilePath);
        public static void CreateAPIFile() => File.Create(apiKeyFilePath);
        internal void SetPlaylistID(string url) => playListID = GetPlaylistIDFromURL(url);

        private string GetAPIKeyFromFile()
        {
            if (!File.Exists(apiKeyFilePath))
                throw new Exception("Can't get API key from file. api.txt file does not exist. Current path: " + apiKeyFilePath);

            string text = File.ReadAllText(apiKeyFilePath);
            if (String.IsNullOrEmpty(text))
                Logger.Log("Can't get API key from file. There was no text inside api.txt");

            return text;
        }

        private string GetPlaylistIDFromURL(string url)
        {
            bool isUrlValid = IsUrlValid(url, out string playlistId);
            return playlistId;
        }

        private bool IsUrlValid(string url, out string playlistId)
        {
            //PlayList ID is everything after the equal sign
            string[] split = url.Split('=');
            playlistId = split[split.Length - 1];

            if (String.IsNullOrEmpty(playListID))
                return false;

            const int playlistIDLength = 34;
            if (playlistId.Length != playlistIDLength)
            {
                Logger.Log("Playlist ID must be 34 char long. Current length: " + playListID.Length);
                return false;
            }

            return true;
        }

        private bool CanGetPlaylistFromJSON()
        {
            if (String.IsNullOrEmpty(playListID))
                return false;

            if (string.IsNullOrEmpty(APIKey))
            {
                Logger.Log("You need to put your api key in \"api.txt\", located in the same folder as this program");
                return false;
            }

            return true;
        }

        internal string GetJsonFromYouTube()
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

            WebReader webReader = new WebReader();
            string json = null;

            json = webReader.ReadText_FromURL(fullUrl);
            return json;
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