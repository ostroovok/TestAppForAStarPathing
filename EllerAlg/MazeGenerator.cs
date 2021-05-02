using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EllerAlg
{
    public class MazeGenerator
    {

        private int[] _right;
        private int[] _bot;
        private int _counter = 0;
        public int Width { get; }
        public List<Cell[]> Maze { get; private set; }

        private Random _rnd;

        public MazeGenerator(int width)
        {
            Width = width;

            _rnd = new Random();

            Maze = new List<Cell[]>
            {
                new Cell[Width]
            };
            for (int j = 0; j < Width; j++)
                Maze.Last()[j] = new Cell(new Vector2Int(Maze.Count, j));

            _right = new int[Width];
            _bot = new int[Width];

            for (int i = 0; i < Width; i++)
            {
                _right[i] = i;
                _bot[i] = i;
            }

        }

        public void Generate()
        {
             Maze.Add(CreateOneRow(_counter, _right, _bot));
        }
        #region Private Methods
        private Cell[] CreateOneRow(int i, int[] right, int[] bot)
        {
            var temp = new Cell[Width];

            for (int c = 0; c < Width; c++)
            {
                temp[c] = new Cell(new Vector2Int(i, c));
            }

            for (int j = 0; j < Width; j++)
            {
                if (j != Width - 1 && j + 1 != right[j] && _rnd.NextDouble() < 0.5)
                {
                    temp[j].Right = false;
                    temp[j + 1].Left = false;

                    right[bot[j + 1]] = right[j];

                    bot[right[j]] = bot[j + 1];

                    right[j] = j + 1;

                    bot[j + 1] = j;
                }

                if (j != right[j] && _rnd.NextDouble() < 0.5)
                {
                    right[bot[j]] = right[j];

                    bot[right[j]] = bot[j];

                    right[j] = j;

                    bot[j] = j;
                }
                else
                {
                    temp[j].Bottom = false;
                    if (!Maze[i - 1][j].Bottom)
                        temp[j].Top = false;
                }
                    
            }
            return temp;
        }

        private void PrintWithOutNumbers(int start, int h, Cell[] maze)
        {
            for (int i = start; i < h; i++)
            {
                for (int j = 0; j < maze.Length; j++)
                {
                    if (maze[j].Bottom)
                    {
                        Console.Write("__");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    if (maze[j].Right)
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        ConsoleColor color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(".");
                        Console.ForegroundColor = color;
                    }
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
