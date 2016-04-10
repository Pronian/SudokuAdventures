using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuAdv.Logic
{
    public class GameBoardLogic : ViewBase
    {
        //const string FileName = "gameboard.dat";

        public SquareViewLogic SelectedBox { get; set; }
        public SquareViewLogic[,] GameArray { get; set; }
        public int EmptyBoxes { get; private set; }

        private string solution;

        /// <summary>
        /// Changes the value of the selected box and validates the board.
        /// </summary>
        /// <param name="inputValue">The value to be entered, 0 if the box is to be cleared.</param>
        /// <returns>True if the board is completed and valid, othervise false.</returns>
        public int SendInput(int inputValue)
        {
            int status = 0; // 0 means Normal condionons, nothing changes
                            // 1 means exit game
                            // 2 means add mistake

            if (SelectedBox != null)
            {
                //board.SetCell(SelectedBox.Row, SelectedBox.Column, inputValue);
                if (inputValue != 0 && SelectedBox.Value == 0) // user clears square
                {
                    EmptyBoxes--;
                }
                else if (inputValue == 0 && SelectedBox.Value != 0) //input into square
                {
                    EmptyBoxes++;
                }
                SelectedBox.Value = inputValue; //actual value change 
                if (EmptyBoxes == 0) // candidate for solved puzzle
                {
                    if (CheckSolution())
                    {
                        return 1;
                    }
                }
                else // check if input is correct
                {
                    if (!ValidateInput()) status = 2;
                }
                SelectedBox.IsSelected = false;
                SelectedBox = null;

            }

            return status;
        }

        /// <summary>
        /// Sets up the game board and solution with the given puzzle.
        /// </summary>
        /// <param name="puzzle">The sudoku puzzle to be entered and solved.</param>
        /// <returns>The setup board along with the solution.</returns>
        private static GameBoardLogic FillNewPuzzle(string puzzle)
        {
            GameBoardLogic result = new GameBoardLogic();
            result.EmptyBoxes = 0;

            List<SquareViewLogic> squares = new List<SquareViewLogic>();
            foreach (char s in puzzle.ToCharArray())
            {
                SquareViewLogic square = new SquareViewLogic();
                if (s != '0' && s != '.')
                {
                    square.Value = int.Parse(s.ToString());
                    square.IsEditable = false;
                }
                else
                {
                    square.IsEditable = true;
                    result.EmptyBoxes++;
                }
                squares.Add(square);
            }

            result.GameArray = LoadFromSquareList(squares);

            RuleBasedSolver solv = new RuleBasedSolver(puzzle);
            solv.RunStep(10000);
            result.solution = solv.board.ToString();

            return result;
        }

        public static GameBoardLogic LoadNewPuzzle(int id)
        {
            string puzzle = Data.PuzzleReader.GetPuzzleById(id);

            return FillNewPuzzle(puzzle);
        }

        /// <summary>
        /// Generates a new puzzle and returns its board.
        /// </summary>
        /// <returns>The generated and setup board along with the solution.</returns>
        public static GameBoardLogic LoadRandomPuzzle()
        {
            Generator gen = new Generator();
            Random rand = new Random();
            int r = rand.Next(28, 34);
            //Logic.PopupManager.ShowWaitPopup("Generating puzzle with " + r + " clues.");
            string result = gen.GeneratePuzzleMultithreaded(r, 500);
            
            return FillNewPuzzle(result);
        }

        /// <summary>
        /// Returns the puzzle in the form of a string, where 0 means an empty place.
        /// </summary>
        /// <returns>The puzzle in the form of a string, where 0 means an empty place.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (SquareViewLogic item in GameArray)
            {
                result.Append(item.Value.ToString());
            }
            return result.ToString();
        }

        /// <summary>
        /// Enters the correct solution into the board.
        /// </summary>
        public void Solve()
        {
            SendInput(0);   //deselect item
            Clear();
            int counter = 0;
            foreach (var box in GameArray)
            {
                if (box.IsEditable) 
                {
                    box.Value = solution[counter] - '0';
                }
                counter++;
            }
        }

        /// <summary>
        /// Removes all user input from the board.
        /// </summary>
        public void Clear()
        {
            foreach (SquareViewLogic item in GameArray)
            {
                if (item.IsEditable)
                {
                    item.Value = 0;
                    item.IsValid = true;
                }
            }
        }

        /// <summary>
        /// Checks weather the current solution is correct.
        /// </summary>
        /// <returns>True if the solution is correct, false otherwise.</returns>
        public bool CheckSolution()
        {
            if (ToString() == solution)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Marks the invalid squares as such.
        /// </summary>
        public bool ValidateBoard()
        {
            bool isValid = true;
            int counter = 0;
            foreach (var box in GameArray)
	        {
                if (box.Value != 0 && box.Value != (solution[counter] - '0') )
                {
                    box.IsValid = false;
                    isValid = false;
                }
                else
                {
                    box.IsValid = true;
                }
                counter++;
	        }

            return isValid;
        }

        public bool ValidateInput()
        {
            bool isValid = true;

            int inputPlace = SelectedBox.Column + SelectedBox.Row * 9;

            if (SelectedBox.Value != 0 && SelectedBox.Value != (solution[inputPlace] - '0') )
            {
                SelectedBox.IsValid = false;
                isValid = false;
            }
            else
            {
                SelectedBox.IsValid = true;
            }

            return isValid;
        }

        private static SquareViewLogic[,] LoadFromSquareList(List<SquareViewLogic> list)
        {
            SquareViewLogic[,] result = new SquareViewLogic[9, 9];
            int counter = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    result[i, j] = list[counter];
                    result[i, j].Row = i;
                    result[i, j].Column = j;
                    counter += 1;
                }
            }

            return result;
        }

        public void DisableBoard()
        {
            foreach (var box in GameArray)
            {
                box.IsEditable = false;
            }
        }
    }
}
