using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace SudokuAdv
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"/View/PlayPage.xaml", UriKind.Relative));
            //SudokuAdv.Data.PuzzleDataClassesDataContext dc = new SudokuAdv.Data.PuzzleDataClassesDataContext(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='E:\Documents\Visual Studio 2013\Projects\SudokuAdv\SudokuAdv\Data\Database.mdf';Integrated Security=True;Connect Timeout=30");
           // var ex = dc.DatabaseExists();
        }

    }
}