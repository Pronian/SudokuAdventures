using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace SudokuAdv.Data
{
    class PuzzleReader
    {
        private static string fileName = @"puzzles.dat";
        private static string[] fileContent;

        //private async static Task LD()
        //{
        //    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + fileName));
        //    using (Stream stream = (await file.OpenReadAsync()).AsStreamForRead())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        Windows.Storage.FileProperties.BasicProperties x = await file.GetBasicPropertiesAsync();
        //        char[] delimiter = { '\n' };
        //        fileContent = reader.ReadToEnd().Split(delimiter);
        //    }
        //}

        private static void LoadFile()
        {
            var streamResourceInfo = App.GetResourceStream(new Uri(fileName, UriKind.Relative));

            using (var stream = streamResourceInfo.Stream)
            {
                using (var streamReader = new StreamReader(stream))
                {
                    //TestFileContentTextBox.Text = streamReader.ReadToEnd();
                    char[] delimiter = { '\n' };
                    fileContent = streamReader.ReadToEnd().Split(delimiter);
                }
            }
        }

        public static string GetPuzzleById(int id)
        {
            LoadFile();

            char[] delimiter = { '\t' };
            int position = fileContent.Length / 2;
            int step = fileContent.Length / 4;
            string result = "";

            while (result == "") 
            {
                string[] line = fileContent[position].Split(delimiter);
                int i;
                int.TryParse(line[0], out i);

                if(i == id)
                {
                    result = line[1];
                }
                else if (id < i)
                {
                    position -= step;
                }
                else if (id > i)
                {
                    position += step;
                }

                if(result == "" && (position == 0 || position == fileContent.Length )) break;
                if(step > 1) step = step / 2;
            }

            return result;
        }
    }
}
