using System;
using System.Timers;
using System.Windows;
using System.Windows.Media.Animation;
using GiveBack_Hackathon.Lib;
using GiveBack_Hackathon.Wpf.UserControls;

namespace GiveBack_Hackathon.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;

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
    }
}
