using GiveBack_Hackathon.Lib.YouTube;
using GiveBack_Hackathon.Wpf.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GiveBack_Hackathon.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;
        PlaylistViewer_UserControl playlistViewer;

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new Logger(); //Create new logger so the singleton can initialize
            Logger.Log("Welcome to the YouTube Playlist Tracker");
        }

        private void ToolbarButton1_Click(object sender, RoutedEventArgs e)
        {
            CreateFakePlaylists();
            if (playlistViewer is null)
            {
                playlistViewer = new PlaylistViewer_UserControl();
                AddVideos();
            }

            var children = ContentGrid.Children;
            if (children.Count > 0)
            {
                var test = children[0] as PlaylistViewer_UserControl;
                if (test != null)
                {
                    children.RemoveAt(0);
                }
            }
            else
            {
                ContentGrid.Children.Add(playlistViewer);
            }
        }
        
        public void AddVideos()
        {
            var viewer = playlistViewer.PlaylistViewer;

            Playlist playlist = new Playlist();
            playlist = playlist.LoadFromFile(Playlist.playlistDir + "\\test.json");
            

            for (int i = 0; i < playlist.PlaylistVideos.Count; i++)
            {
                var video = new PlaylistItem_UserControl();
                video.videoName = i.ToString();
                video.index = (i + 1);                // Incrementing it by 1 fixes 0 base indeing for the user 
                video.Width = ContentGrid.ActualWidth;
                video.Height = (ContentGrid.ActualHeight / 10);

                ListBoxItem item = new ListBoxItem();
                //item.Padding = new Thickness(15, 3, 0, 0);
                item.Content = video;
                viewer.Children.Add(item);
            }
        }

        private void CreateFakePlaylists()
        {
            const int numFakePlaylists = 5;
            for (int i = 0; i < numFakePlaylists; i++)
            {
                Button playlistButton = new Button();
                playlistButton.Width = Playlist_ListBox.ActualWidth - 15;
                playlistButton.Background = Brushes.Orange;
                playlistButton.Foreground = Brushes.Black;

                playlistButton.Content = i.ToString();
                
                playlistButton.Click += PlaylistButton_Click;

                ListBoxItem item = new ListBoxItem();
                item.Content = playlistButton;
                Playlist_ListBox.Items.Add(item);
            }
        }

        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            
			
        }

        private void TestPlaylist()
        {
            /*Playlist playlist = new Playlist();
            playlist = playlist.LoadFromFile(Playlist.playlistDir + "\\test.json");
            Logger.Log(playlist.PlaylistVideos[0].Title);*/
        }
    }
}
