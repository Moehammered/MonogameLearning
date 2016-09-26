using MazeEscape.Utilities;
using System;
using System.IO;

namespace MazeEscape.GameUtilities
{
    struct LevelData
    {
        public int columns, rows;
        private int[] data;

        public LevelData(int columns, int rows)
        {
            this.columns = columns;
            this.rows = rows;
            data = new int[rows * columns];
        }

        public void initialiseData()
        {
            data = new int[rows * columns];
        }

        public void setData(int column, int row, int value)
        {
            data[row * columns + column] = value;
        }

        public int getData(int column, int row)
        {
            return data[row * columns + column];
        }
    }

    class LevelLoader
    {
        private FileLoader file;
        private FileInfo[] levels;

        public LevelLoader()
        {
            file = new FileLoader();
        }

        public void loadLevelFiles()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory + "/Levels");
            try
            {
                levels = dir.GetFiles();
                Console.WriteLine("Levels found: " + levels.Length);
            }
            catch(DirectoryNotFoundException)
            {
                levels = new FileInfo[0];
            }
        }

        public bool loadLevel(string levelName, out LevelData data)
        {
            data = new LevelData(0, 0);
            if(levels.Length > 0)
            {
                foreach(FileInfo level in levels)
                {
                    if(level.Name.ToLower() == levelName.ToLower())
                    {
                        //found the level, perform the loading stuff
                        if (parseLevel(level.FullName, out data))
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            return false;
        }

        private bool parseLevel(string path, out LevelData level)
        {
            Console.WriteLine("Level file path: " + path);
            string[] lines = file.readTextFileLines(path);
            level = new LevelData();

            if (lines != null && lines.Length > 0)
            {
                //no error handling or validity for now
                int maxColumns = lines[0].Length;
                int maxRows = lines.Length;
                level.columns = maxColumns;
                level.rows = maxRows;
                level.initialiseData();
                int x;
                for (int i = 0; i < lines.Length; i++)
                {
                    x = 0;
                    while (x < lines[i].Length && x < maxColumns)
                    {
                        int value = Convert.ToInt32(lines[i][x]) - 48;

                        level.setData(x, i, value);
                        x++;
                    }
                }
            }
            else
                return false;

            return true;
        }
    }
}
