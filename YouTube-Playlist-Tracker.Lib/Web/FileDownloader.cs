﻿using System;
using System.IO;
using System.Net;
using System.Threading;

namespace YouTube_Playlist_Tracker.Lib.Web
{
    /// <summary>
    /// Class for downloading files from the internet
    /// </summary>
    public class FileDownloader
    {
        /// <summary>
        /// Download file from URL. Needs to be run on a thread if using UI, otherwise the UI will freeze
        /// </summary>
        public void DownloadFile(string url, string dest)
        {
            Guard.ThrowIfArgumentIsNull(url, "Can't download file, URL is null", "url");
            Guard.ThrowIfArgumentIsNull(dest, "Can't download file, destination is null", "dest");

            dest += GetFilenameFromURL(url);
            string tempDir = CreateTempDir();
            string tempDest = tempDir + "\\file";

            var client = new WebClient();
            for (int i = 0; i < 150; i++)
            {
                try { client.DownloadFile(url, tempDest); break; } catch { }
                Thread.Sleep(50);
            }

            CreateDownloadFolder(dest);
            File.Move(tempDest, dest);
            Directory.Delete(tempDir, true);
        }

        private string GetFilenameFromURL(string url)
        {
            string[] split = url.Split('/');
            string fileName = split[split.Length - 1];
            return fileName;
        }

        private string CreateTempDir()
        {
            string tempDir = Environment.CurrentDirectory + "\\temp";
            Directory.CreateDirectory(tempDir);
            return tempDir;
        }

        private string CreateDownloadFolder(string dest)
        {
            FileInfo fileInfo = new FileInfo(dest);
            var destDir = fileInfo.FullName.Replace(fileInfo.Name, "");
            Directory.CreateDirectory(destDir);

            return destDir;
        }
    }
}
