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
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using SudokuAdv.Logic;

namespace SudokuAdv.View
{
    public partial class SquareView : UserControl
    {
        private SquareViewLogic _viewLogic;
        public SquareView(SquareViewLogic viewModel)
        {
            InitializeComponent();
            _viewLogic = viewModel;
            this.DataContext = _viewLogic;
            _viewLogic.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(viewModel_PropertyChanged);
            SetColors();
            SetThickness();
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentBoxState":
                    SetColors();
                    break;
            }
        }

        private void SetColors()
        {

            SolidColorBrush sYellow = new SolidColorBrush();
            sYellow.Color = ConvertStringToColor("#FFF8C67E");

            SolidColorBrush sEmbed = new SolidColorBrush();
            sEmbed.Color = ConvertStringToColor("#B0559595");

            SolidColorBrush sAccentNorm = new SolidColorBrush();
            sAccentNorm.Color = ConvertStringToColor("#FFEB7C6B");

            SolidColorBrush sErr = new SolidColorBrush();
            sErr.Color = ConvertStringToColor("#FF903453");

            switch (_viewLogic.CurrentBoxState)
            {
                case BoxStates.UnEditable:
                    MainText.Foreground = sEmbed;
                    LayoutRoot.Background = sYellow;
                    break;
                case BoxStates.Invalid:
                    MainText.Foreground = sErr;
                    LayoutRoot.Background = sYellow;
                    VibrateController.Default.Start(TimeSpan.FromMilliseconds(200));
                    break;
                case BoxStates.Selected:
                    MainText.Foreground = sYellow;
                    LayoutRoot.Background = sAccentNorm;
                    break;
                default:
                    MainText.Foreground = sAccentNorm;
                    LayoutRoot.Background = sYellow;
                    break;
            }
        }

        private void SetThickness()
        {
            int topBorder = 1;
            int rightBorder = 1;
            int leftBorder = 1;
            int bottomBorder = 1;
            if (_viewLogic.Row % 3 == 0)
                topBorder = 3;
            if (_viewLogic.Column % 3 == 0)
                leftBorder = 3;
            if (_viewLogic.Row == 8)
                bottomBorder = 3;
            if (_viewLogic.Column == 8)
                rightBorder = 3;

            BoxGridBorder.BorderThickness = new Thickness(leftBorder, topBorder, rightBorder, bottomBorder);
        }

        public event EventHandler BoxClicked;

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (BoxClicked != null)
                BoxClicked(this, null);
        }

        public static Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

    }
}
