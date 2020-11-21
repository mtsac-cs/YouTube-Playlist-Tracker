using System;
using System.IO;
using System.Net;
using System.Threading;

namespace GiveBack_Hackathon.Lib
{
    /// <summary>
    /// Contains methods relating to the internet, such as reading text from a webpage or downloading a file
    /// </summary>
    public class WebHandler
    {
        /// <summary>
        /// Downloads string of text from the URL. Needs to be run on a thread if using UI, otherwise the UI will freeze
        /// </summary>
        /// <param name="url">URL to get string of text from</param>
        /// <returns>String of text or nothing</returns>
        public static string ReadText_FromURL(string url)
        {
            Guard.ThrowIfArgumentIsNull(url, "Can't read text from url. Url argument is null", "url");

            WebClient client = new WebClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers.Add("user-agent", " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");

            string result = "";
            string lastExeption = "";
            for (int i = 0; i <= 250; i++)
            {
                try
                {
                    result = client.DownloadString(url);
                    if (String.IsNullOrEmpty(result))
                        continue;

                    break;
                }
                catch (Exception e)
                {
                    if (e.Message != lastExeption)
                    {
                        Logger.Log(e.Message);
                        lastExeption = e.Message;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Download file from URL. Needs to be run on a thread if using UI, otherwise the UI will freeze
        /// </summary>
        /// <param name="url">download url</param>
        /// <param name="dest">file destination. Just needs directory, doesn't need file name</param>
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
                try { client.DownloadFile(url, tempDest); break; }
                catch { Thread.Sleep(50); }
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
            Directory.CreateDirectory(tempDir); //creates dir if not found
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
