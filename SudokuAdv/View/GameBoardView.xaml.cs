using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SudokuAdv.Logic;

namespace SudokuAdv.View
{
    public partial class GameBoardView : UserControl
    {
        public GameBoardView()
        {
            InitializeComponent();
        }

        private GameBoardLogic _gameBoard;
        public GameBoardLogic GameBoard
        {
            get { return _gameBoard; }
            set
            {
                _gameBoard = value;
                BindBoard();
            }
        }

        #region Private Methods

        private void ChildBoxClicked(object sender, EventArgs e)
        {
            SquareViewLogic inputSquare = (SquareViewLogic)((SquareView)sender).DataContext;
            if (GameBoard.SelectedBox != null)
            {
                GameBoard.SelectedBox.IsSelected = false;
            }

            if (GameBoard.SelectedBox == inputSquare || !inputSquare.IsEditable)
            {
                GameBoard.SelectedBox = null;
            }
            else
            {
                GameBoard.SelectedBox = inputSquare;
                GameBoard.SelectedBox.IsSelected = true;
            }
        }

        private void BindBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    SquareViewLogic square = GameBoard.GameArray[i, j];
                    SquareView uiSquare = new SquareView(square);
                    LayoutRoot.Children.Add(uiSquare);
                    uiSquare.BoxClicked += new EventHandler(ChildBoxClicked);

                    Grid.SetRow(uiSquare, i);
                    Grid.SetColumn(uiSquare, j);

                    uiSquare.DataContext = square;
                }
            }
        }

        #endregion
    }
}
