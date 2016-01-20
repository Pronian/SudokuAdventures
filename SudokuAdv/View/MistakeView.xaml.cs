using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SudokuAdv.View
{
    public partial class MistakeView : UserControl
    {
        private static BitmapImage noMistPath = new BitmapImage(new Uri(@"/Images/NoMistake.png", UriKind.Relative));
        private static BitmapImage mistPath = new BitmapImage(new Uri(@"/Images/Mistake.png", UriKind.Relative));

        public int MistakeNumber { get; private set; }

        public MistakeView()
        {
            InitializeComponent();
            MistakeNumber = 0;
        }

        public void Clear()
        {
            MistakeNumber = 0;
            img1.Source = noMistPath;
            img2.Source = noMistPath;
            img3.Source = noMistPath;
        }

        public bool AddMistake()
        {
            switch (MistakeNumber)
            {
                case 0:
                    MistakeNumber++;
                    img1.Source = mistPath;
                    return true;
                case 1:
                    MistakeNumber++;
                    img2.Source = mistPath;
                    return true;
                case 2:
                    MistakeNumber++;
                    img3.Source = mistPath;
                    return false;
                default:
                    return false;
            }

        }
    }
}
