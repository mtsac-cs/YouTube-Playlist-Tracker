using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YouTube_Playlist_Tracker.Wpf.UserControls
{
    /// <summary>
    /// Interaction logic for PlaylistItem_UserControl.xaml
    /// </summary>
    public partial class PlaylistItem_UserControl : UserControl
    {

        public string videoName;
        public int index;

        public PlaylistItem_UserControl()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            VideoName_TextBlock.Text = videoName;
            PlaylistNumber_TextBlock.Text = index.ToString();
        }
    }
}
