using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SudokuAdv.Logic;

namespace SudokuAdv.View
{
    public partial class GamePage : PhoneApplicationPage
    {
        private enum GameMode { Campaign = 0, Beginner=1, Easy=2, Medium=3, Hard=4, VeryHard=5 }
        private GameMode mode;
        private bool isComplete = false;
        private DispatcherTimer timer;
        private DateTime startTime;

        public GamePage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
            InputControl.SendInput += new EventHandler(InputControl_SendInput);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(OnTimerTick);
        }

        void InputControl_SendInput(object sender, EventArgs e)
        {
            int status = MainBoard.GameBoard.SendInput((int)sender);
            if (status == 1)
            {
                timer.Stop();
                isComplete = true;
                PopupManager.ShowWinPopup();
                MainBoard.GameBoard.DisableBoard();
            }
            else if(status == 2)
            {
                MistakeView.AddMistake();
                if (MistakeView.MistakeNumber > 2) 
                {
                    timer.Stop();
                    PopupManager.ShowLosePopup();
                    MainBoard.GameBoard.DisableBoard();
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GameBoardLogic board;
            int id = 5;

            if (NavigationContext.QueryString.ContainsKey("campaign"))
            {
                id = Data.PuzzleManager.GetPuzzleID(0);
                board = GameBoardLogic.LoadNewPuzzle(id);
                txtPuzzNum.Text = "Campaign " + (Data.PuzzleManager.PuzzleNumber + 1)  + @"/" + Data.PuzzleIDs.All[0].Length;
                mode = GameMode.Campaign;
            }
            else if (NavigationContext.QueryString.ContainsKey("beginner"))
            {
                id = Data.PuzzleManager.GetPuzzleID(1);
                board = GameBoardLogic.LoadNewPuzzle(id);
                txtPuzzNum.Text = "Beginner " + (Data.PuzzleManager.PuzzleNumber + 1) + @"/" + Data.PuzzleIDs.All[1].Length;
                mode = GameMode.Beginner;
            }
            else if (NavigationContext.QueryString.ContainsKey("easy"))
            {
                id = Data.PuzzleManager.GetPuzzleID(2);
                board = GameBoardLogic.LoadNewPuzzle(id);
                txtPuzzNum.Text = "Beginner " + (Data.PuzzleManager.PuzzleNumber + 1) + @"/" + Data.PuzzleIDs.All[2].Length;
                mode = GameMode.Easy;
            }
            else if (NavigationContext.QueryString.ContainsKey("medium"))
            {
                id = Data.PuzzleManager.GetPuzzleID(3);
                board = GameBoardLogic.LoadNewPuzzle(id);
                txtPuzzNum.Text = "Beginner " + (Data.PuzzleManager.PuzzleNumber + 1) + @"/" + Data.PuzzleIDs.All[3].Length;
                mode = GameMode.Medium;
            }
            else if (NavigationContext.QueryString.ContainsKey("hard"))
            {
                id = Data.PuzzleManager.GetPuzzleID(4);
                board = GameBoardLogic.LoadNewPuzzle(id);
                txtPuzzNum.Text = "Beginner " + (Data.PuzzleManager.PuzzleNumber + 1) + @"/" + Data.PuzzleIDs.All[4].Length;
                mode = GameMode.Hard;
            }
            else if (NavigationContext.QueryString.ContainsKey("very"))
            {
                id = Data.PuzzleManager.GetPuzzleID(5);
                board = GameBoardLogic.LoadNewPuzzle(id);
                txtPuzzNum.Text = "Beginner " + (Data.PuzzleManager.PuzzleNumber + 1) + @"/" + Data.PuzzleIDs.All[5].Length;
                mode = GameMode.VeryHard;
            }
            else if (NavigationContext.QueryString.ContainsKey("random"))
            {
                board = GameBoardLogic.LoadRandomPuzzle();
                txtPuzzNum.Text = "Random Puzzle";
            }
            else
            {
                board = GameBoardLogic.LoadNewPuzzle(5);
            }

            MainBoard.GameBoard = board;

            Logic.PopupManager.ClosePopup();

            startTime = DateTime.Now;
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Logic.PopupManager.ClosePopup();
            if (isComplete)
	        {
                switch (mode)
                {
                    case GameMode.Campaign:
                        Data.PuzzleManager.IncrementSelection(0);
                        break;
                    case GameMode.Beginner:
                        Data.PuzzleManager.IncrementSelection(1);
                        break;
                    case GameMode.Easy:
                        Data.PuzzleManager.IncrementSelection(2);
                        break;
                    case GameMode.Medium:
                        Data.PuzzleManager.IncrementSelection(3);
                        break;
                    case GameMode.Hard:
                        Data.PuzzleManager.IncrementSelection(4);
                        break;
                    case GameMode.VeryHard:
                        Data.PuzzleManager.IncrementSelection(5);
                        break;
                    default:
                        break;
                }
	        }
            base.OnNavigatedFrom(e);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            switch (e.Orientation)
            {
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeLeft:
                case PageOrientation.LandscapeRight:
                    TitlePanel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(InputControl, 1);
                    Grid.SetRow(InputControl, 0);
                    InputControl.RotateVertical();
                    break;
                case PageOrientation.Portrait:
                case PageOrientation.PortraitUp:
                case PageOrientation.PortraitDown:
                    TitlePanel.Visibility = Visibility.Visible;
                    Grid.SetColumn(InputControl, 0);
                    Grid.SetRow(InputControl, 1);
                    InputControl.RotateHorizontal();
                    break;
                default:
                    break;
            }
            base.OnOrientationChanged(e);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            TimeSpan current = (DateTime.Now - startTime);
            if (current.Hours < 1)
            {
                txtTime.Text = current.ToString(@"mm\:ss");
            }
            else
            {
                txtTime.Text = current.ToString(@"hh\:mm\:ss");
            }
            
        }
        

        private void Solve_Click(object sender, EventArgs e)
        {
            MainBoard.GameBoard.Solve();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            MainBoard.GameBoard.Clear();
        }
    }
}