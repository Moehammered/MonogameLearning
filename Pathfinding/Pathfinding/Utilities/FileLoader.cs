using System;
using System.IO;

namespace MazeEscape.Utilities
{
    class FileLoader
    {
        public FileLoader()
        {
        }

        public byte[] openFile(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch(FileNotFoundException exc)
            {
                return null;
            }
        }

        public string openTextFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch(FileNotFoundException exc)
            {
                return string.Empty;
            }
        }

        public string[] readTextFileLines(string path)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch(FileNotFoundException exc)
            {
                return null;
            }
        }
    }
}
