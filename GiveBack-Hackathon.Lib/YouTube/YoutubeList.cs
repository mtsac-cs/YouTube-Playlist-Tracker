using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;

namespace GiveBack_Hackathon.Lib.YouTube
{
    public class YoutubeList
    {
        //Variable Declarations:
        private const int videoListSize = 100;
        private string APIKey;
        private string playListID;

        //Function Definitions:
        public YoutubeList(string playListURL = "")
        {
            //Set API Key
            APIKey = "";

            //Set Playlist ID
            setPlayListURL(playListURL);
        }


        //Get the PlayList ID from its corrisponding URL
        private string obtainID(string url)
        {
            //PlayList ID is everything after the equal sign
            string[] split = url.Split('=');
            string playlistID = split[split.Length - 1];

            //return the ID
            return playlistID;
        }

        //set the PlayList ID from an input playList URL
        public void setPlayListURL(string url)
        {
            //store the ID from the provided url
            playListID = obtainID(url);
            return;
        }

        //Print out help for the user
        public void PrintHelp()
        {
            //Output Messages
            Logger.Log("This program lists all video names in the specified YouTube playlist.");
            Logger.Log("USAGE: Please Enter A Valid Youtube PlayList Link");
        }

        public void dostuff()
        {
        
        }

        //API Call to get the current videos in the specifies playList
        //private async Task<dynamic> GetVideosInPlaylistAsync(string playListId)
        private dynamic GetVideosInPlaylistAsync(string playListId)
        {
            //Dictionary Object
            var parameters = new Dictionary<string, string>
            {
                //Store API Key
                ["key"] = APIKey,
                //Store the Playlisy ID
                ["playlistId"] = playListId,
                //Get only the info in this part
                ["part"] = "snippet",
                //get on the info in this feild for this part
                ["fields"] = "pageInfo, items/snippet(title)",
                //Max number of video you can pull
                ["maxResults"] = videoListSize.ToString()
            };

            //Create URL
            var baseUrl = "https://www.googleapis.com/youtube/v3/playlistItems?";
            var fullUrl = MakeUrlWithQuery(baseUrl, parameters);

            //Create new Client Object to file request
            WebHandler webHandler = new WebHandler();
            var result = webHandler.ReadText_FromURL(fullUrl);
            MessageBox.Show(result);
            //var result = await new HttpClient().GetStringAsync(fullUrl);

            //Run only if successful creation of client object
            if (result != null)
            {
                //Deserialize - convert strings to data types
                return JsonConvert.DeserializeObject(result);
            }

            //return
            return default(dynamic);
        }

        //Get the string array holding all of the video titles
        public string[] getTitleList()
        {
            //Get the videos in the playlist
            var result = GetVideosInPlaylistAsync(playListID).Result;
            string[] videoList = new string[videoListSize];

            //Get count and output value to the user
            var count = result.items.Count;
            Logger.Log($"Total items in playlist: {result.pageInfo.totalResults,2}");
            Logger.Log($"Public items in playlist: {count,2}");

            //Check to see if at least one video was found
            var i = 0;
            if (count > 0)
            {
                //loop through each element in the playlist
                foreach (var item in result.items)
                {
                    videoList[i] = $"{item.snippet.title}";
                }
            }

            //return the Video List
            return videoList;
        }

        //Make a url with a query
        private string MakeUrlWithQuery(string baseUrl,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            //null string case
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            //Empty string case
            if (parameters == null || parameters.Count() == 0)
                return baseUrl;

            //Return the full Url
            return parameters.Aggregate(baseUrl,
                (accumulated, kvp) => string.Format($"{accumulated}{kvp.Key}={kvp.Value}&"));
        }
    }
}