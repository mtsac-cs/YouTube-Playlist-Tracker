using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;

namespace GiveBack_Hackathon.Lib.YouTube
{
    public class Playlist
    {
        public static string playlistDir = Environment.CurrentDirectory + "\\playlist data";
        public List<YoutubeVideo> PlaylistVideos { get; set; } = new List<YoutubeVideo>();

        

        public Playlist LoadFromFile(string path)
        {
            if (!IsPathValid(path))
                return null;

            string json = File.ReadAllText(path);
            if (String.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<Playlist>(json);
        }

        private bool IsPathValid(string path)
        {
            Guard.ThrowIfArgumentIsNull(path, "Can't load playlist from file, path to file is null", "path");
            if (!File.Exists(path))
                return false;

            return true;
        }


        public void SaveToFile(string savePath)
        {
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
