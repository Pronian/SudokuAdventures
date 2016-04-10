using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;

namespace SudokuAdv.Data
{
    static class PuzzleManager
    {
        private const string FileName = "save.dat";

        //private static int last_campaign = 0;
        //private static int last_beginner = 0;
        //private static int last_easy = 0;
        //private static int last_medium = 0;
        //private static int last_hard = 0;
        //private static int last_veryhard = 0;
        private static int[] last_played = { 0, 0, 0, 0, 0, 0 };

        public static int PuzzleNumber { get; private set; }

        private static void LoadLastPlayed()
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(FileName))
                {
                    using (IsolatedStorageFileStream stream = store.OpenFile(FileName, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string conent = reader.ReadToEnd();
                            string[] lines = conent.Split('\n');

                            for (int i = 0; i < last_played.Length; i++)
                            {
                                last_played[i] = int.Parse(lines[i]);
                            }
                        }
                    }
                }
            }
        }

        private static void SaveLastPlayed()
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(FileName))
                {
                    store.DeleteFile(FileName);
                }

                using (IsolatedStorageFileStream stream = store.CreateFile(FileName))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        for (int i = 0; i < last_played.Length; i++)
                        {
                            writer.WriteLine(last_played[i].ToString());
                        }
                    }
                }
            }
        }
        
        public static int GetPuzzleID(int selection)
        {
            LoadLastPlayed();
            PuzzleNumber = last_played[selection];
            return PuzzleIDs.All[selection][PuzzleNumber];
        }

        public static void IncrementSelection(int selection)
        {

            if (last_played[selection] >= PuzzleIDs.All[selection].Length - 1) //all puzzles played
            {
                last_played[selection] = 0;
            }
            else
            {
                last_played[selection]++;
            }
                
            SaveLastPlayed();
        }
    }
}
