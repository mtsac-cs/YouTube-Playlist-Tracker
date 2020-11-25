using YouTube_Playlist_Tracker.Lib.YouTube;
using System;
using System.Windows;

namespace YouTube_Playlist_Tracker.Wpf.Windows
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
            if (!YoutubeApi.DoesApiFileExist())
            {
                YoutubeApi.CreateAPIFile();
                Logger.Log("Paste your api key in the \"api.txt\" file, in the same Directory as this program");
                return;
            }

            if (!IsPlaylistUrlValid(PlaylistURL_TextBox.Text))
                return;

            Playlists playlists = new Playlists();
            playlists.GetPlaylistFromYoutube(PlaylistName_TextBox.Text, PlaylistURL_TextBox.Text);
            Close();
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
    }
}
