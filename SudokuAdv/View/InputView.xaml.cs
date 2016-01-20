using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SudokuAdv.View
{
    public partial class InputView : UserControl
    {
        public InputView()
        {
            InitializeComponent();
        }

        public event EventHandler SendInput;

        private void UserInput_Click(object sender, RoutedEventArgs e)
        {
            int inputValue = int.Parse(((Button)sender).Tag.ToString());
            if (SendInput != null)
                SendInput(inputValue, null);
        }

        public void RotateVertical()
        {
            TopRow.Orientation = Orientation.Vertical;
            BottomRow.Orientation = Orientation.Vertical;
            OuterPanel.Orientation = Orientation.Horizontal;
        }

        public void RotateHorizontal()
        {
            TopRow.Orientation = Orientation.Horizontal;
            BottomRow.Orientation = Orientation.Horizontal;
            OuterPanel.Orientation = Orientation.Vertical;
        }
    }
}
