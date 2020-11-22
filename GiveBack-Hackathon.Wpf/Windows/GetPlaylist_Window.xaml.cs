using GiveBack_Hackathon.Lib.YouTube;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace GiveBack_Hackathon.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for UserInput.xaml
    /// </summary>
    public partial class GetPlaylist_Window : Window
    {
        public GetPlaylist_Window()
        {
            InitializeComponent();
            MainWindow.instance.Closed += CloseThisWindow;
        }

        private void CloseThisWindow(object sender, EventArgs e)
        {
            Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string playlistUrl = SearchTextBox.Text;
            if (!IsPlaylistUrlValid(playlistUrl))
                return;

            GetPlaylistFromYoutube(playlistUrl);
        }

        private bool IsPlaylistUrlValid(string playlistUrl)
        {
            if (String.IsNullOrEmpty(playlistUrl))
            {
                ShowError("Error! You need to enter a playlist URL to continue");
                return false;
            }

            if (!playlistUrl.ToLower().Contains("youtube") && !playlistUrl.ToLower().Contains("youtu.be"))
            {
                ShowError("The URL you entered doesn't appear to be associated with YouTube. Please enter a YouTube URL");
                return false;
            }

            return true;
        }

        private void ShowError(string errorMessage)
        {
            Logger.Log(errorMessage);
            MessageBox.Show(errorMessage);
        }

        private void GetPlaylistFromYoutube(string url)
        {
            YoutubeList video = new YoutubeList(url);

            Thread t = new Thread(() => 
            {
                var titles = video.getTitleList();
                
                Playlist p = new Playlist("new saving.json");
                p.PlaylistVideos = titles;
                p.SaveToFile();
            });

            t.IsBackground = true;
            t.Start();
        }
    }
}
