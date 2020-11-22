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
        public string playlistDir = PlaylistInfo.playlistDir;
        public List<PlaylistInfo> allPlaylists;
        public Playlists()
        {
            Directory.CreateDirectory(playlistDir);
        }

        public static PlaylistInfo GetPlaylistFromPath(string fileName)
        {
            if (!fileName.ToLower().EndsWith(".json"))
                fileName += ".json";

            string path = (PlaylistInfo.playlistDir + "//" + fileName).Replace("//","\\");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Can't get playlist from file, fileName is invalid. fileName: " + path, "path");

            if (!File.Exists(path))
                throw new ArgumentException("Can't get playlist from file, file doesn't exist. Path: " + path, "path");

            PlaylistInfo loadedPlaylist = new PlaylistInfo(fileName);
            return loadedPlaylist;
        }

        public void LoadAllPlaylists()
        {
            if (!DoPlaylistsExist(out FileInfo[] playlistFiles))
                return;

            foreach (var item in playlistFiles)
            {
                PlaylistInfo p = new PlaylistInfo(item.Name);
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

        public void AddPlaylist(PlaylistInfo playlist)
        {
            MainWindow.instance.AddPlaylistToListbox(playlist);
        }
    }
}
