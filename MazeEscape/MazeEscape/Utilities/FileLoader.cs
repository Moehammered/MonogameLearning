using System.IO;

namespace MonogameLearning.Utilities
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
            catch(FileNotFoundException)
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
            catch(FileNotFoundException)
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
            catch(FileNotFoundException)
            {
                return null;
            }
        }
    }
}
