using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SudokuAdv.View
{
    public partial class WaitPopup : UserControl
    {
        public WaitPopup()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = Message;
            this.txtInfo.SetBinding(TextBlock.TextProperty, binding);
        }

        public static string Message = "Please wait.";
    }
}
