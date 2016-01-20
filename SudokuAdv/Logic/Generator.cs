using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuAdv.Logic
{
    class Generator
    {
        public RuleBasedSolver solver;
        public int Solutions { get; private set; }
        public int ClueNumer { get; private set; }
        public int Difficulty { get; private set; }

        private List<string> candidateList = new List<string>();
        private Random rand = new Random();
        private int solve_time;
        private string gen_puzzle;
        private bool threadsOn;


        private void TryGeneratePuzzle(int clueN)
        {
            if (clueN < 17 || clueN > 80)
            {
                throw new ArgumentException();
            }

            ClueNumer = clueN;

            Random rand = new Random();
            solver = new RuleBasedSolver(new Board());
            solver.board.AnalyzePossibilities();
            int row, col;
            int fill = 81;
            List<int> entries = new List<int>();

            int whileCount = 0;
            int ifCount = 0;

            while (fill > 0)
            {
                whileCount++;
                row = rand.Next(0, 9);
                col = rand.Next(0, 9);
                if (solver.board.board[row, col][0] == 0)
                {
                    int rPossibNumber = rand.Next(1, solver.board.board[row, col].Count);
                    if (solver.board.SetCell(row, col, solver.board.board[row, col][rPossibNumber]) == false)
                    {
                        ifCount++;
                        solver.board.board[row, col][0] = 0;
                        int r = rand.Next(0, entries.Count);
                        row = entries[r] / 9;
                        col = entries[r] % 9;
                        entries.Remove(entries[r]);
                        solver.board.board[row, col][0] = 0;
                        fill++;


                    }
                    else
                    {
                        entries.Add(row * 9 + col);
                        fill--;
                    }
                    solver.board.AnalyzePossibilities();

                }
            }
            for (int i = 0; i < (81 - clueN); i++)
            {
                row = rand.Next(0, 9);
                col = rand.Next(0, 9);
                if (solver.board.board[row, col][0] != 0)
                {
                    solver.board.board[row, col][0] = 0;
                }
                else
                {
                    i--;
                }

            }
            solver.board.AnalyzePossibilities();
        }

        /// <summary>
        /// Returns a valid sudoku with the given number of clues.
        /// </summary>
        /// <param name="clueNumber">The number of clues the board will have.</param>
        /// <param name="mili_time">The maximum time in miliseconds to check a single candidate.</param>
        /// <returns>The generated puzzle in the form of a string.</returns>
        public string GeneratePuzzle(int clueNumber, int mili_time)
        {
            string candidate;

            do
            {
                TryGeneratePuzzle(clueNumber);
                candidate = ToString();
                solver.StopOnMultipleSolutions = true;
                Solutions = solver.RunStep(mili_time);
            } while (Solutions != 1);

            return candidate;

        }

        /// <summary>
        /// Returns a valid sudoku with a random number of clues within the given range. 
        /// </summary>
        /// <param name="min_clue">The minimum inclusive number of clues.</param>
        /// <param name="max_clue">The maximum exclusive number of clues.</param>
        /// <param name="mili_time">The maximum time in miliseconds to check a single candidate.</param>
        /// <returns>The generated puzzle in the form of a string.</returns>
        public string GeneratePuzzle(int min_clue, int max_clue, int mili_time)
        {
            int clueNumber = rand.Next(min_clue, max_clue);

            return GeneratePuzzle(clueNumber, mili_time);
        }


        private void GeneratePuzzle()
        {
            Random rand = new Random();
            solver = new RuleBasedSolver(new Board());
            solver.board.AnalyzePossibilities();


            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (solver.board.board[row, col][0] == 0)
                    {
                        int rPossibNumber = rand.Next(1, solver.board.board[row, col].Count);
                        if (solver.board.SetCell(row, col, solver.board.board[row, col][rPossibNumber]) == false)
                        {
                            solver.board.board[row, col][0] = 0;
                            solver.board.AnalyzePossibilities();
                            col -= 2;
                            //guess++;
                        }
                    }

                }

            }

        }

        public override string ToString()
        {
            return solver.board.ToString();
        }

        private void ConsumeSolve()
        {
            int solutions = 0;
            string candidate = "";
            RuleBasedSolver rbs = new RuleBasedSolver();

            while (solutions != 1)
            {
                Monitor.Enter(candidateList);
                if (candidateList.Count > 0)
                {
                    candidate = String.Copy(candidateList[0]);
                    candidateList.RemoveAt(0);
                    Monitor.Exit(candidateList);

                    rbs.board.SetBoard(candidate);
                    rbs.StopOnMultipleSolutions = true;
                    solutions = rbs.RunStep(solve_time);

                }
                else
                {
                    //Console.WriteLine("Consumer: List empty!");
                    Monitor.Exit(candidateList);
                    Thread.Sleep(50);
                }
            }

            gen_puzzle = candidate;
            Difficulty = rbs.CalculateDifficulty();
        }

        private void Produce()
        {
            Generator gen = new Generator();
            string candidate = "";
            threadsOn = true;

            while (threadsOn)
            {
                if (candidate == "")
                {
                    gen.TryGeneratePuzzle(ClueNumer);
                    candidate = gen.ToString();
                }
                Monitor.Enter(candidateList);
                if (candidateList.Count < 20)
                {
                    candidateList.Add(candidate);

                    Monitor.Exit(candidateList);
                    candidate = "";
                }
                else
                {
                    Monitor.Exit(candidateList);
                    Thread.Sleep(50);
                }

            }
        }

        public string GeneratePuzzleMultithreaded(int clueNumber, int mili_time)
        {
            solve_time = mili_time;
            ClueNumer = clueNumber;
            string candidate = "";
            Thread consumer = new Thread(new ThreadStart(ConsumeSolve));
            consumer.Name = "Generator: Solver Thread";
            Thread producer = new Thread(new ThreadStart(Produce));
            producer.Name = "Generator: Producer Thread";
            //Thread producer2 = new Thread(new ThreadStart(Produce));
            consumer.Start();
            producer.Start();
            //producer2.Start();

            while (consumer.IsAlive)
            {
                if (candidate == "")
                {
                    TryGeneratePuzzle(ClueNumer);
                    candidate = ToString();
                }
                //mutex.WaitOne();
                Monitor.Enter(candidateList);
                if (candidateList.Count < 20)
                {
                    candidateList.Add(candidate);
                    //mutex.ReleaseMutex();
                    Monitor.Exit(candidateList);
                    candidate = "";
                    //Monitor.PulseAll(candidateList);
                }
                else
                {

                    // mutex.ReleaseMutex();
                    Monitor.Exit(candidateList);
                    //Monitor.Wait(candidateList);
                    Thread.Sleep(50);
                }
            }
            threadsOn = false;

            candidateList.Clear();

            return gen_puzzle;
        }

        public string GeneratePuzzleMultithreaded(int min_clue, int max_clue, int mili_time)
        {
            int clues = rand.Next(min_clue, max_clue);
            return GeneratePuzzleMultithreaded(clues, mili_time);
        }
    }
}
