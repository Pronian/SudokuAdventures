using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuAdv.Logic
{
    class RuleBasedSolver
    {
        public Board board;
        /// <summary>
        /// The number of times the Single rule was used succesfully.
        /// </summary>
        public int SingleCount = 0;
        /// <summary>
        /// The number of times the Naked rule was used succesfully.
        /// </summary>
        public int NakedCount = 0;
        /// <summary>
        /// The number of times the guessing was used.
        /// </summary>
        public int GuessCount = 0;

        public bool StopOnMultipleSolutions = false;

        private List<int>[,] regions = new List<int>[27, 9];
        private DateTime EndTime;

        public RuleBasedSolver()
        {
            board = new Board();
        }

        /// <summary>
        /// Constructor which initialises the puzzle with the given grid.
        /// </summary>
        /// <param name="grid">A 9 by 9 int array which describes a puzzle grid. It has to be 9 by 9.</param>
        public RuleBasedSolver(int[,] grid)
        {
            board = new Board();
            board.SetBoard(grid);
        }

        /// <summary>
        /// Constructor for copying a board to the new RuleBasedSolver object. 
        /// </summary>
        /// <param name="b">The board to be copied.</param>
        public RuleBasedSolver(Board b)
        {
            board = new Board(b);
        }

        public RuleBasedSolver(string puzzle)
        {
            board = new Board();
            board.SetBoard(puzzle);
        }


        private void CreateReferences()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    regions[i, j] = board.board[i, j];
                    regions[j + 9, i] = board.board[i, j];
                }
            }
            for (int b = 0; b < 9; b++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int ishift = 3 * (b / 3);
                        int jshift = 3 * (b % 3);
                        regions[b + 18, i * 3 + j] = board.board[i + ishift, j + jshift];
                    }
                }
            }
        }

        /// <summary>
        /// Applies the rule for single Candidate. This means that there is a single candidate in a square.
        /// </summary>
        /// <returns>True if the rule was applyable.</returns>
        public bool SingleRule()
        {
            bool match = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board.board[i, j].Count == 2)
                    {
                        match = true;
                        SingleCount++;
                        board.SetCell(i, j, board.board[i, j][1]);
                    }
                }
            }
            return match;
        }

        /// <summary>
        /// Finds all combinations from an int vector containing r numbers and beginning with a number with least index i.
        /// </summary>
        /// <remarks>
        /// The method finds the combinations by recursively calling itself
        /// and changing i and r. The combinations are then concatenated into an
        /// list consisting of the combinations which are of the type list<int>.
        /// </remarks>
        /// <param name="n">The list from which the numbers in the combination will come from.</param>
        /// <param name="r">The number of numbers that shall be picked from n.</param>
        /// <param name="i">The least index a number can have. i=0 means that any number could be picked.</param>
        /// <returns>list<list<int>> which contains r−sized vectors in a vector containing all possible combinations found.</returns>
        private List<List<int>> FindCombinations(List<int> n, int r, int i)
        {
            if (r == 0)
            {
                List<List<int>> x = new List<List<int>>();
                x.Add(new List<int>());
                return x;
            }
            else if (i >= n.Count)
            {
                return new List<List<int>>();
            }

            List<List<int>> combinations = new List<List<int>>();
            List<List<int>> a;
            List<List<int>> b;

            a = FindCombinations(n, r - 1, i + 1);

            for (int t = 0; t < a.Count; t++)
            {
                a[t].Add(n[i]);
                combinations.Add(a[t]);
            }

            b = FindCombinations(n, r, i + 1);

            for (int t = 0; t < b.Count; t++)
            {
                combinations.Add(b[t]);
            }

            return combinations;
        }

        /// <summary>
        /// Applies the rule of naked/hidden tuples to a specific region.
        /// </summary>
        /// <param name="rn">rn is the current row in the regions array.</param>
        /// <returns>true if the rule was applyable for the specific region row.</returns>
        private bool NakedRegion(int rn)
        {
            bool match = false;

            List<int> n = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (regions[rn, i][0] == 0)
                {
                    n.Add(i);
                }
            }

            //Loop through naked pair , triple , quadruple
            //This also includes hidden single, pair , triple , quad ...
            for (int r = 2; r <= 8; r++)
            {
                List<List<int>> comb = FindCombinations(n, r, 0);

                for (int c = 0; c < comb.Count; c++)
                {
                    bool[] numbers = { false, false, false, false, false, false, false, false, false };
                    for (int i = 0; i < comb[c].Count; i++)
                    {
                        int squarei = comb[c][i];
                        for (int j = 1; j < regions[rn, squarei].Count; j++)
                        {
                            numbers[regions[rn, squarei][j] - 1] = true;
                        }
                    }

                    int count = 0;
                    for (int t = 0; t < 9; t++)
                    {
                        if (numbers[t]) { count++; }
                    }
                    if (count <= r)
                    {
                        //Found naked pair , triple...
                        //But it may have already been found previously, so match is not set to true
                        for (int i = 0; i < 9; i++)
                        {
                            // Search if i is contained in found pair, triple...
                            bool skip = false;
                            for (int t = 0; t < comb[c].Count; t++)
                            {
                                if (comb[c][t] == i)
                                {
                                    skip = true;
                                    break;
                                }
                            }
                            if (skip) continue;

                            for (int j = 1; j < regions[rn, i].Count; j++)
                            {
                                if (numbers[regions[rn, i][j] - 1])
                                {
                                    regions[rn, i].RemoveAt(j);
                                    j--; // compensate for removal
                                    // Something changed so match is true
                                    match = true;
                                }
                            }
                        }
                    }
                }
            }

            return match;
        }

        /// <summary>
        /// Applies the rule of hidden and naked pairs, triples and up to octuples.
        /// </summary>
        /// <remarks>
        /// Note that naked and hidden tuples are the same rule but in reverse.
        /// This means that if there is a set of squares that together form
        /// a hidden tuple than the others quares in that region is a naked tuple.
        /// Therefore, one only needs to check for naked tuples.
        /// </remarks>
        /// <returns>True if the rule was applyable.</returns>
        public bool NakedRule()
        {
            bool match = false;

            for (int i = 0; i < 27; i++)
            {
                if (NakedRegion(i))
                {
                    match = true;
                    NakedCount++;
                    return true;
                }
            }

            return match;
        }


        /// <summary>
        /// Guesses the content of one cell by using a brute force algorithm and continues with standard solving.
        /// </summary>
        /// <returns>Retuns the number of valid solutions.</returns>
        public int Guess()
        {
            //Find square with least possibilities.
            int[] min = { 100, 0, 0 }; // [min , i , j ];
            int solutions = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board.board[i, j][0] == 0 && min[0] > board.board[i, j].Count)
                    {
                        min[0] = board.board[i, j].Count;
                        min[1] = i; min[2] = j;
                    }
                }
            }
            if (min[0] == 100)
            {
                return 1;
            }
            if (board.board[min[1], min[2]].Count == 1)
            {
                return 0;
            }
            List<Board> CorrectGuesses = new List<Board>();
            for (int g_index = 1; g_index < board.board[min[1], min[2]].Count; g_index++)
            {
                int g = board.board[min[1], min[2]][g_index];
                Board tmp = new Board(board);
                List<int> tmpList = tmp.board[min[1], min[2]];
                tmpList.Clear();
                tmpList.Add(g);
                tmp.UpdateCellPossibilities(min[1], min[2]);
                RuleBasedSolver solver = new RuleBasedSolver(tmp);
                if (StopOnMultipleSolutions)
                {
                    solver.StopOnMultipleSolutions = true;
                }
                solutions = solver.RunStep(EndTime);
                this.SingleCount += solver.SingleCount;
                this.NakedCount += solver.SingleCount;
                this.GuessCount += solver.GuessCount;
                if (StopOnMultipleSolutions && solutions > 1)
                {
                    return solutions;
                }
                while (solutions > 0)
                {
                    CorrectGuesses.Add(solver.board);
                    solutions--;
                }

            }

            if (CorrectGuesses.Count == 0)
            {
                return 0;
            }
            else
            {
                GuessCount++;
                board = new Board(CorrectGuesses[0]);
                return CorrectGuesses.Count;
            }
        }

        /// <summary>
        /// Applies the rules that solve the puzzles.
        /// Consideres the endingtime for solutions and returns if this time is exceeded
        /// </summary>
        /// <returns>The number of solutions</returns>
        public int ApplyRules()
        {
            if (DateTime.Now > EndTime)
            {
                return 0;
            }

            CreateReferences();

            while (true)
            {
                //The easy rules first.
                if (SingleRule()) continue;
                if (NakedRule()) continue;
                break;
            }

            return Guess();
        }

        /// <summary>
        /// Solves the puzzle and returns more than 0 if succesfull.
        /// There is also a time limit which must be hold.
        /// </summary>
        /// <param name="ts">The solving time in miliseconds</param>
        /// <returns>The number of solutions</returns>
        public int RunStep(int ts)
        {
            EndTime = DateTime.Now + TimeSpan.FromMilliseconds(ts);
            return ApplyRules();
        }

        /// <summary>
        /// Used in the recursive guess function for maintainig the solving time through the instances.
        /// </summary>
        /// <param name="dt">DateTime variable decsribing when the solver should stop.</param>
        /// <returns>The number of solutions</returns>
        private int RunStep(DateTime dt)
        {
            EndTime = dt;
            return ApplyRules();
        }

        public int CalculateDifficulty()
        {
            int dif = SingleCount + NakedCount * 25 + GuessCount * 150;
            return dif;
        }
    }
}
