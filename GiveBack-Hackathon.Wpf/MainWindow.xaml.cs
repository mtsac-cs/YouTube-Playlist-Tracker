using GiveBack_Hackathon.Wpf.UserControls;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
            for (int i = 0; i < 20; i++)
            {
                var video = new PlaylistItem_UserControl();
                video.videoName = i.ToString();
                video.index = i;
                video.Width = ContentGrid.ActualWidth;
                video.Height = ContentGrid.ActualHeight / 10;

                ListBoxItem item = new ListBoxItem();
                item.Padding = new Thickness(15, 3, 0, 0);
                item.Content = video;
                viewer.Children.Add(item);
            }
        }

        private void AddPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();


        }

        private void TutorialButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();



        }

    }
}
