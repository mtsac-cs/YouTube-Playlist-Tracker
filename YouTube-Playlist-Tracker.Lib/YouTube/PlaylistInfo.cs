using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace YouTube_Playlist_Tracker.Lib.YouTube
{
    public class PlaylistInfo
    {
        private string fileName;
        public string playlistName;
        public string playlistID;
        public List<VideoInfo> PlaylistVideos = new List<VideoInfo>();
        public static string playlistDir = Environment.CurrentDirectory + "\\playlist data";

        #region Constructors
        internal PlaylistInfo()
        {

        }

        public PlaylistInfo(string fileName)
        {
            this.fileName = fileName;

            var loadedPlaylist = LoadFromFile();
            if (loadedPlaylist is null)
                return;

            playlistName = loadedPlaylist.playlistName;
            PlaylistVideos = loadedPlaylist.PlaylistVideos;
        }
        #endregion


        public PlaylistInfo LoadFromFile() => LoadFromFile(playlistDir + "//" + fileName);
        public PlaylistInfo LoadFromFile(string filePath)
        {
            if (!IsPathValid(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            if (String.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<PlaylistInfo>(json);
        }

        private bool IsPathValid(string filePath)
        {
            Guard.ThrowIfArgumentIsNull(filePath, "Can't load playlist from file, path to file is null", "filePath");
            if (!File.Exists(filePath))
                return false;

            return true;
        }

        public void SaveToFile()
        {
            if (!fileName.EndsWith(".json"))
                fileName += ".json";

            string savePath = playlistDir + "//" + fileName;
            Guard.ThrowIfArgumentIsNull(savePath, "Can't save playlist to file, save path is null", "savePath");
            CreateDirIfNotFound(savePath);
            string output = JsonConvert.SerializeObject(this, Formatting.Indented);

            StreamWriter serialize = new StreamWriter(savePath, false);
            serialize.Write(output);
            serialize.Close();
        }

        private void CreateDirIfNotFound(string dir)
        {
            FileInfo f = new FileInfo(dir);
            Directory.CreateDirectory(f.Directory.FullName);
        }


        public void GetPlaylistFromYoutube(string url)
        {
            YoutubePlaylistAPI video = new YoutubePlaylistAPI(url);

            var playlist = video.GetPlaylistInfo();
            if (playlist is null)
                return;

            playlistID = playlist.playlistID;
            PlaylistVideos = playlist.PlaylistVideos;
            SaveToFile();
        }
    }
}
