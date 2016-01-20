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
    public partial class LevelsPage : PhoneApplicationPage
    {
        public LevelsPage()
        {
            InitializeComponent();
        }

        private void btnBeginner_Click(object sender, RoutedEventArgs e)
        {
            Logic.PopupManager.ShowWaitPopup();
            this.NavigationService.Navigate(new Uri("/View/GamePage.xaml?beginner", UriKind.Relative));
        }

        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            Logic.PopupManager.ShowWaitPopup();
            this.NavigationService.Navigate(new Uri("/View/GamePage.xaml?easy", UriKind.Relative));
        }

        private void btnMedium_Click(object sender, RoutedEventArgs e)
        {
            Logic.PopupManager.ShowWaitPopup();
            this.NavigationService.Navigate(new Uri("/View/GamePage.xaml?medium", UriKind.Relative));
        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            Logic.PopupManager.ShowWaitPopup();
            this.NavigationService.Navigate(new Uri("/View/GamePage.xaml?hard", UriKind.Relative));
        }

        private void btnVeryHard_Click(object sender, RoutedEventArgs e)
        {
            Logic.PopupManager.ShowWaitPopup();
            this.NavigationService.Navigate(new Uri("/View/GamePage.xaml?very", UriKind.Relative));
        }
    }
}