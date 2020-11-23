using YouTube_Playlist_Tracker.Lib.YouTube;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace YouTube_Playlist_Tracker.Wpf
{
    public class Playlists
    {
        public string playlistDir = PlaylistData.playlistDir;
        public List<PlaylistData> allPlaylists;
        public Playlists()
        {
            Directory.CreateDirectory(playlistDir);
        }

        public static PlaylistData GetPlaylistFromPath(string fileName)
        {
            if (!fileName.ToLower().EndsWith(".json"))
                fileName += ".json";

            string path = (PlaylistData.playlistDir + "//" + fileName).Replace("//","\\");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Can't get playlist from file, fileName is invalid. fileName: " + path, "path");

            if (!File.Exists(path))
                throw new ArgumentException("Can't get playlist from file, file doesn't exist. Path: " + path, "path");

            PlaylistData loadedPlaylist = new PlaylistData(fileName);
            return loadedPlaylist;
        }

        public void LoadAllPlaylists()
        {
            if (!DoPlaylistsExist(out FileInfo[] playlistFiles))
                return;

            foreach (var item in playlistFiles)
            {
                PlaylistData p = new PlaylistData(item.Name);
                AddPlaylist(p);
            }
        }

        private bool DoPlaylistsExist(out FileInfo[] playlistFiles)
        {
            playlistFiles = new FileInfo[0];
            if (!Directory.Exists(playlistDir))
                return false;

            playlistFiles = new DirectoryInfo(playlistDir).GetFiles();
            if (playlistFiles.Length == 0)
                return false;

            return true;
        }

        public void AddPlaylist(PlaylistData playlist)
        {
            MainWindow.instance.AddPlaylistToListbox(playlist);
        }
    }
}
