using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuAdv
{
    class SudokuPuzzle
    {
        public string seed = "ecdfgbaih|fghaiecdb|iabcdhefg|heidbfgac|gfahecibd|dbcgiahef|ifabhgcde|ecgdaibhf|bhdfceagi|1234567890nn";
        public string problem = "53_6___98|_7_195___|_______6_|8__4__7__|_6_8_3_2_|__3__1__6|_6_______|___419_8_|28___5_79";
        public string numberSeed;
        public string solution;

        public string GetSymbol(bool useSeed, int X, int Y)
        {
            int add8 = X / 3;
            int add1 = X - add8;
            int add24 = Y / 3;
            int add3 = Y - add24;
            int result = add1 + 3 * add3 + 8 * add8 + 24 * add24;

            if (useSeed)
            {
                return seed.Substring(result, 1);
            }
            else
            {
                return numberSeed.Substring(result, 1);
            }
        }

        public void resetSolution()
        {
            solution = problem;
        }

        public void UpdateNumberSeed()
        {
            numberSeed = seed;
            string letters = "abcdefghi";

            for (int i = 0; i < 9; i++)
            {
                numberSeed = numberSeed.Replace(letters.Substring(i, 1), seed.Substring(90 + i, 1));
            }
        }

        public void RotateNumberSeed(int timesToRotate)
        {
            string[,] arr = new string[9, 9];

            for (int r = 0; r < timesToRotate; r++)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        arr[j, i] = GetSymbol(false, j, 9 - i - 1);
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }

    }
}
