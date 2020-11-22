using YouTube_Playlist_Tracker.Lib.YouTube;
using YouTube_Playlist_Tracker.Wpf.UserControls;
using YouTube_Playlist_Tracker.Wpf.Windows;
using System;
using System.IO;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace YouTube_Playlist_Tracker.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;
        public static Playlists allPlaylists = new Playlists();
        public string lastLoadedPlaylist;
        PlaylistViewer_UserControl playlistViewer = new PlaylistViewer_UserControl();

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
            CreateAPIFile();


            allPlaylists.LoadAllPlaylists();
        }

        private void CreateAPIFile()
        {
            string path = YoutubeList.apiKeyFilePath;
            if (!File.Exists(path))
                File.Create(path);
        }
        
        private void AddNewPlaylist_Button_Click(object sender, RoutedEventArgs e)
        {
            GetPlaylist_Window getPlaylist = new GetPlaylist_Window();
            getPlaylist.Show();
        }

        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            CreatePlaylistViewerIfNull();
            var playlistButton = sender as Button;
            string playlistName = playlistButton.Content.ToString();
            
            if (playlistName == lastLoadedPlaylist)
                return;

            playlistViewer.PlaylistViewer.Children.Clear();
            var playlist = GetPlaylistFromTitle(playlistName);
            lastLoadedPlaylist = playlistName;
            AddPlaylistVideosToListBox(playlist);
        }

        private void CreatePlaylistViewerIfNull()
        {
            if (!playlistViewer.IsVisible)
            {
                int welcomeScreenIndex = 0;
                ContentGrid.Children.RemoveAt(welcomeScreenIndex);
                ContentGrid.Children.Add(playlistViewer);
            }
        }

        private Playlist GetPlaylistFromTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Can't get playlist from title because title is null", "title");

            var loadedPlaylist = Playlists.GetPlaylistFromPath(title);
            return loadedPlaylist;
        }

        public void AddPlaylistToListbox(Playlist playlist)
        {
            var listBoxItem = CreatePlaylistListBoxItem(playlist);
            Playlist_ListBox.Items.Add(listBoxItem);
        }

        private ListBoxItem CreatePlaylistListBoxItem(Playlist playlist)
        {
            const int sideMargin = 15;
            Button playlistButton = new Button();
            playlistButton.Width = Playlist_ListBox.ActualWidth - sideMargin;
            playlistButton.Background = Brushes.White;
            playlistButton.Foreground = Brushes.Black;
            playlistButton.Click += PlaylistButton_Click;

            playlistButton.Content = playlist.playlistName;

            ListBoxItem item = new ListBoxItem();
            item.Content = playlistButton;
            return item;
        }

        private void AddPlaylistVideosToListBox(Playlist playlist)
        {
            int i = 0;
            var videos = playlist.PlaylistVideos;
            foreach (var video in videos)
            {
                i++;
                video.IndexInPlaylist = i;
                var listBoxItem = CreateVideoListboxItem(video);
                playlistViewer.PlaylistViewer.Children.Add(listBoxItem);
            }
        }

        private ListBoxItem CreateVideoListboxItem(YoutubeVideo video)
        {
            var playlistItem = new PlaylistItem_UserControl();
            playlistItem.videoName = video.Title;
            playlistItem.index = video.IndexInPlaylist;

            const int maxSeenOnScreen = 12;
            playlistItem.Height = ContentGrid.ActualHeight / maxSeenOnScreen;

            const int distanceFromRightEdge = 75;
            playlistItem.Width = ContentGrid.ActualWidth - distanceFromRightEdge;

            const int spaceBetweenSides = 25;
            playlistItem.Margin = new Thickness(spaceBetweenSides, 0, -spaceBetweenSides, 0);
            ListBoxItem item = new ListBoxItem();
            
            item.Content = playlistItem;
            return item;
        }
    }
}
