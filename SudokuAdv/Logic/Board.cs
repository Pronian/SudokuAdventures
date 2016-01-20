using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuAdv.Logic
{
    class Board
    {
        public List<int>[,] board = new List<int>[9, 9];

        public Board()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = new List<int>();
                    board[i, j].Add(0);
                }
            }
        }

        public Board(Board b)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i,j] = new List<int>();
                    for (int k = 0; k < b.board[i,j].Count; k++)
                    {
                        board[i, j].Add(b.board[i, j][k]);
                    }
                }
            }
        }

        /// <summary>
        /// Set the board to the specified grid.
        /// Throws ArgumentException when the parameter size isn't 9 by 9.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// When the parameter size isn't 9 by 9
        /// </exception>
        /// <param name="grid">A 9 by 9 int array which describes a puzzle grid. It has to be 9 by 9.</param>
        public void SetBoard(int[,] grid)
        {
            if (grid.GetLength(0) != 9 || grid.GetLength(1) != 9)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = new List<int>();
                    board[i, j].Add(grid[i, j]);
                }
            }
            AnalyzePossibilities();
        }

        public void SetBoard(string puzzle)
        {
            if (puzzle.Length != 81)
            {
                throw new ArgumentException();
            }
            int i = 0, j = 0, n = 0;
            for (int s = 0; s < 81; s++)
            {
                board[i, j] = new List<int>();
                Int32.TryParse(puzzle.Substring(s, 1), out n );
                board[i, j].Add(n);
                j++;
                if (j > 8)
                {
                    j = 0;
                    i++;
                }
            }
            AnalyzePossibilities();
        }

        /// <summary>
        /// Resets all possibilities and recreates those from the constraints in the puzzle.
        /// </summary>
        /// <remarks>
        /// The possibilities are stored as vectors from index 1 in the board array.
        /// A square which could be either a 1 or 3 will therefore have
        /// the vector {0,1,3} assigned to it. The 0 is because the
        /// square have not yet been assigned any number.
        /// </remarks>
        public void AnalyzePossibilities()
        {
            //Erase old possibility data
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int temp = board[i, j][0];
                    board[i, j].Clear();
                    board[i, j].Add(temp);

                    if (board[i, j][0] != 0)
                    {
                        continue;
                    }

                    for (int x = 1; x <= 9; x++)
                    {
                        board[i, j].Add(x);
                    }
                }
            }

            //Remove wrong possibility data
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j][0] == 0)
                    {
                        continue;
                    }

                    for (int k = 0; k < 9; k++) //Remove from rows
                    {
                        if (board[i, k][0] != 0)
                        {
                            continue;
                        }

                        if (board[i, k].Contains(board[i, j][0]))
                        {
                            board[i, k].Remove(board[i, j][0]);
                        }

                    }

                    for (int k = 0; k < 9; k++) //Remove from colons
                    {
                        if (board[k, j][0] != 0)
                        {
                            continue;
                        }
                        if (board[k, j].Contains(board[i, j][0]))
                        {
                            board[k, j].Remove(board[i, j][0]);
                        }
                    }

                    for (int m = 0; m < 3; m++) // Remove from boxes
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            if (board[m + (i / 3) * 3, n + (j / 3) * 3][0] != 0)
                            {
                                continue;
                            }
                            if (board[m + (i / 3) * 3, n + (j / 3) * 3].Contains(board[i, j][0]))
                            {
                                board[m + (i / 3) * 3, n + (j / 3) * 3].Remove(board[i, j][0]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if the board is valid and completed (solved).
        /// </summary>
        /// <returns>True if completely solved and false otherwise.</returns>
        public bool IsCompleted()
        {
            AnalyzePossibilities();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j][0] == 0) // Check if cell is filled
                    {
                        return false;
                    }

                    for (int k = 0; k < 9; k++) // Check if number is already present in row
                    {
                        if (j == k)
                        {
                            continue;
                        }
                        if (board[i, j][0] == board[i, k][0])
                        {
                            return false;
                        }

                    }

                    for (int k = 0; k < 9; k++) // Check if number is already present in colon
                    {
                        if (i == k)
                        {
                            continue;
                        }
                        if (board[k, j][0] == board[i, j][0])
                        {
                            return false;
                        }
                    }

                    for (int m = 0; m < 3; m++) // Check if number is already present in box
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            if ((m + (i / 3) * 3 == i) && (n + (j / 3) * 3 == j))
                            {
                                continue;
                            }
                            if (board[m + (i / 3) * 3, n + (j / 3) * 3][0] == board[i, j][0])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Check if the board is valid, works if not yet solved.
        /// </summary>
        /// <returns>True if there are no collisions and false otherwise.</returns>
        public bool IsValid()
        {
            AnalyzePossibilities();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j][0] == 0) // Check if cell is filled
                    {
                        continue;
                    }

                    for (int k = 0; k < 9; k++) // Check if number is already present in row
                    {
                        if (j == k)
                        {
                            continue;
                        }
                        if (board[i, j][0] == board[i, k][0])
                        {
                            return false;
                        }

                    }

                    for (int k = 0; k < 9; k++) // Check if number is already present in colon
                    {
                        if (i == k)
                        {
                            continue;
                        }
                        if (board[k, j][0] == board[i, j][0])
                        {
                            return false;
                        }
                    }

                    for (int m = 0; m < 3; m++) // Check if number is already present in box
                    {
                        for (int n = 0; n < 3; n++)
                        {
                            if ((m + (i / 3) * 3 == i) && (n + (j / 3) * 3 == j))
                            {
                                continue;
                            }
                            if (board[m + (i / 3) * 3, n + (j / 3) * 3][0] == board[i, j][0])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Removes all candidates in the same row, column and box as the specified cell.
        /// </summary>
        /// <param name="i">y−coordinate of the cell</param>
        /// <param name="j">x−coordinate of the cell</param>
        public bool UpdateCellPossibilities(int i, int j)
        {
            for (int k = 0; k < 9; k++) //Remove from rows
            {
                if (board[i, k][0] != 0)
                {
                    continue;
                }

                if (board[i, k].Contains(board[i, j][0]))
                {
                    board[i, k].Remove(board[i, j][0]);
                }

                if (board[i, k].Count == 1)
                {
                    return false;
                }

            }

            for (int k = 0; k < 9; k++) //Remove from colons
            {
                if (board[k, j][0] != 0)
                {
                    continue;
                }
                if (board[k, j].Contains(board[i, j][0]))
                {
                    board[k, j].Remove(board[i, j][0]);
                }
                if (board[k, j].Count == 1)
                {
                    return false;
                }
            }

            for (int m = 0; m < 3; m++) // Remove from boxes
            {
                for (int n = 0; n < 3; n++)
                {
                    if (board[m + (i / 3) * 3, n + (j / 3) * 3][0] != 0)
                    {
                        continue;
                    }
                    if (board[m + (i / 3) * 3, n + (j / 3) * 3].Contains(board[i, j][0]))
                    {
                        board[m + (i / 3) * 3, n + (j / 3) * 3].Remove(board[i, j][0]);
                    }
                    if (board[m + (i / 3) * 3, n + (j / 3) * 3].Count == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Fill a cell with the given number and updates the possibilites.
        /// </summary>
        /// <param name="x">x−coordinate of the cell</param>
        /// <param name="y">y−coordinate of the cell</param>
        /// <param name="number">The number (1 to 9) to be put into the cell.</param>
        public bool SetCell(int y, int x, int number)
        {
            if (number < 1 || number > 9 || x < 0 || x > 8 || y < 0 || y > 8)
            {
                throw new ArgumentException();
            }

            board[y, x].Clear();
            board[y, x].Add(number);
            return UpdateCellPossibilities(y, x);
        }

        public void printBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write("{0}", board[i,j][0]);
                    if ((j+1) % 3 == 0) Console.Write("|");
                }
                Console.WriteLine();
                if ((i + 1) % 3 == 0)
                Console.WriteLine("---+---+----");
            }
            Console.WriteLine();
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < 9; i++ )
            {
                for (int j = 0; j < 9; j++)
                {
                    result += board[i, j][0].ToString(); 
                }
            }

            return result;

        }
    }
}
