using System;
using System.Collections.Generic;
using System.Threading;

namespace YouTube_Playlist_Tracker.Lib.YouTube
{
    public class PlaylistData
    {
        readonly string fileName;
        public string playlistName;
        public string playlistID;
        public List<VideoData> PlaylistVideos = new List<VideoData>();
        public static string playlistDir = Environment.CurrentDirectory + "\\playlist data";

        #region Constructors
        public PlaylistData(string playlistName)
        {
            this.fileName = playlistName;
            this.playlistName = playlistName.Replace(".json", "");
            
            string filePath = playlistName + "\\" + playlistName;
            var loadedPlaylist = LoadFromFile(filePath);
            if (loadedPlaylist is null)
                return;
            
            PlaylistVideos = loadedPlaylist.PlaylistVideos;
        }
        #endregion

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
