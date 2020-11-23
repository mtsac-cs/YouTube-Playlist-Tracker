using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

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

            playlistTitle = playlistName.Replace(".json", "");
            SetFileName();

            string filePath = playlistDir + "\\" + fileName;
            var loadedPlaylist = LoadFromFile(filePath);
            if (loadedPlaylist is null)
                return;

            PlaylistVideos = loadedPlaylist.PlaylistVideos;
        }
        #endregion
        
        private void SetFileName()
        {
            fileName = playlistTitle;
            if (!fileName.EndsWith(".json"))
                fileName += ".json";
        }

        public PlaylistData LoadFromFile(string filePath)
        {
            var loadedPlaylist = Serializer.LoadFromFile<PlaylistData>(filePath);
            return loadedPlaylist;
        }


        public void SaveToFile() => SaveToFile(playlistDir + "\\" + fileName);
        public void SaveToFile(string savePath)
        {
            Serializer.SaveToFile<PlaylistData>(this, savePath);
        }


        public PlaylistData GetFromYoutube_OnThread(string url)
        {
            Thread t = new Thread(() =>
            {
                GetFromYoutube(url);
            });
            t.IsBackground = true;
            t.Start();
            t.Join(); //this line might cause program to freeze while getting data from web

            return this;
        }

        private PlaylistData GetFromYoutube(string url)
        {
            PlaylistWebReader reader = new PlaylistWebReader(url);
            var config = reader.GetPlaylistConfig();

            PlaylistVideos = reader.GetVideosInPlaylist(config);
            playlistID = config.Items[0].Snippet.PlaylistId;
            return this;
        }
    }
}
