using System.Windows;
using GiveBack_Hackathon.Lib;

namespace GiveBack_Hackathon.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            Logger.MessageLogged += Logger_MessageLogged;
        }
        #endregion


        private void Logger_MessageLogged(object sender, Logger.LogEvents e)
        {
            
        }
    }
}
