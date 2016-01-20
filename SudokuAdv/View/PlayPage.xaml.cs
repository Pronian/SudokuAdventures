using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SudokuAdv
{
    public partial class PlayPage : PhoneApplicationPage
    {
        public PlayPage()
        {
            InitializeComponent();
        }

        private void btnCampaign_Click(object sender, RoutedEventArgs e)
        {
            Logic.PopupManager.ShowWaitPopup();
            this.NavigationService.Navigate(new Uri("/View/GamePage.xaml?campaign", UriKind.Relative));
        }

        private void btnLevels_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/View/LevelsPage.xaml", UriKind.Relative));
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            Logic.PopupManager.ShowWaitPopup();
            this.NavigationService.Navigate(new Uri("/View/GamePage.xaml?random", UriKind.Relative));
        }

        
    }
}