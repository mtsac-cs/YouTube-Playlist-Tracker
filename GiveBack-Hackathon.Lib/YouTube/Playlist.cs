using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GiveBack_Hackathon.Lib.YouTube
{
    public class Playlist
    {
        public static string playlistDir = Environment.CurrentDirectory + "\\playlist data";
        private string fileName;
        public string playlistName;
        public List<YoutubeVideo> PlaylistVideos { get; set; } = new List<YoutubeVideo>();

        public Playlist(string fileName)
        {
            this.fileName = fileName;

            var loadedPlaylist = LoadFromFile();
            if (loadedPlaylist is null)
                return;

            
            playlistName = loadedPlaylist.playlistName;
            PlaylistVideos = loadedPlaylist.PlaylistVideos;
        }

        private Playlist LoadFromFile()
        {
            string filePath = playlistDir + "//" + fileName;
            if (!IsPathValid(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            if (String.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<Playlist>(json);
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
    }
}
