using System;
using System.Collections.Generic;
using System.Threading;

namespace YouTube_Playlist_Tracker.Lib.YouTube
{
    /// <summary>
    /// This is the Playlist class. All playlist info is here
    /// </summary>
    public class PlaylistData
    {
        private string fileName;
        public string playlistTitle;
        public string playlistID;
        public List<VideoData> PlaylistVideos = new List<VideoData>();
        public static string playlistDir = Environment.CurrentDirectory + "\\playlist data";


        #region Constructors
        public PlaylistData(string playlistName)
        {
            if (String.IsNullOrEmpty(playlistName))
                return;

            SetPlaylistInfo(playlistName);
            if (!TryLoadFromFile(out PlaylistData loadedPlaylist))
                return;

            PlaylistVideos = loadedPlaylist.PlaylistVideos;
        }
        #endregion


        private void SetPlaylistInfo(string playlistName)
        {
            playlistTitle = playlistName.Replace(".json", "");
            fileName = playlistTitle;
            if (!fileName.EndsWith(".json"))
                fileName += ".json";
        }

        private bool TryLoadFromFile(out PlaylistData loadedPlaylist)
        {
            string filePath = playlistDir + "\\" + fileName;
            loadedPlaylist = LoadFromFile(filePath);
            if (loadedPlaylist is null)
                return false;

            return true;
        }

        public PlaylistData LoadFromFile() => LoadFromFile(playlistDir + "\\" + fileName);
        public PlaylistData LoadFromFile(string filePath)
        {
            return Serializer.LoadFromFile<PlaylistData>(filePath);
        }

        public void SaveToFile() => SaveToFile(playlistDir + "\\" + fileName);
        public void SaveToFile(string savePath)
        {
            Serializer.SaveToFile<PlaylistData>(this, savePath);
        }

        /// <summary>
        /// Get PlaylistData from Youtube URL. If program is UI, it might freeze if not run on a seperate thread 
        /// </summary>
        public PlaylistData GetFromYoutube(string url)
        {
            YoutubeApiReader reader = new YoutubeApiReader(url);
            var config = reader.GetPlaylistConfig();

            PlaylistVideos = reader.GetVideosInPlaylist(config);
            playlistID = config.Items[0].Snippet.PlaylistId;
            return this;
        }

        /// <summary>
        /// Get PlaylistData from Youtube URL. This is run on a sepearate thread
        /// </summary>
        public PlaylistData GetFromYoutube_OnThread(string url)
        {
            Thread t = new Thread(() => { GetFromYoutube(url); });
            t.IsBackground = true;
            t.Start();
            t.Join(); //this line might cause program to freeze while getting data from web

            return this;
        }
    }
}
