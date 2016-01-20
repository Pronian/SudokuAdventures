using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace SudokuAdv.Logic
{
    class PopupManager
    {
        private static Popup popup = new Popup();
        public static bool PopupIsOpen 
        {
            get
            {
                if (popup != null)
                {
                    return popup.IsOpen;
                }
                else
                    return false;
            }
        }

        public static void ShowWaitPopup()
        {
            popup.VerticalOffset = 400;
            View.WaitPopup wp = new View.WaitPopup();
            popup.Child = wp;
            popup.IsOpen = true;
        }

        public static void ShowWaitPopup(string infoText)
        {
            popup.VerticalOffset = 400;
            View.WaitPopup wp = new View.WaitPopup();
            wp.txtInfo.Text = infoText;
            popup.Child = wp;
            popup.IsOpen = true;
        }

        public static void ShowWinPopup()
        {
            popup.VerticalOffset = 350;
            View.WinPopup wp = new View.WinPopup();
            popup.Child = wp;
            popup.IsOpen = true;

            wp.btnClose.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        public static void ShowLosePopup()
        {
            popup.VerticalOffset = 350;
            View.LosePopup lp = new View.LosePopup();
            popup.Child = lp;
            popup.IsOpen = true;

            lp.btnClose.Click += (s, args) =>
            {
                popup.IsOpen = false;
            };
        }

        public static void UpdateWaitPopup(string infoText)
        {
            if (popup != null)
            {
                popup.IsOpen = false;
                //View.WaitPopup wp = new View.WaitPopup();
                //wp.txtInfo.Text = infoText;
                //popup.Child = wp;
                popup.IsOpen = true;
            }
        }

        public static void ClosePopup()
        {
            if (popup != null) 
            {
                popup.IsOpen = false;
            }
        }
    }
}
