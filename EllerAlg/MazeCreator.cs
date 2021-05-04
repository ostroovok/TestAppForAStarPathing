using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EllerAlg
{
    public class MazeCreator : IMaze
    {
        public int Width { get; }
        public int Height { get; private set; }
        public List<Cell[]> Maze { get; set; }

        private Random _rnd;

        public MazeCreator(int width, int height)
        {
            Width = width;
            Height = height;
            _rnd = new Random();
        }

        public void Generate()
        {
            Maze = new();
            for (int i = 0; i < Height; i++)
            {
                var row = new Cell[Width];
                for (int j = 0; j < Width; j++)
                {
                    row[j] = new Cell(new Vector2Int(i, j));
                }
                Maze.Add(row);
            }

            var temp = new int[Width];
            var bot = new int[Width];
            for (int i = 0; i < Width; i++)
            {
                temp[i] = i;
                bot[i] = i;
            }

            for (int i = 0; i < Height - 1; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (j != Width - 1 && j + 1 != temp[j] && _rnd.NextDouble() < 0.5)
                    {
                        Maze[i][j].Right = false;

                        temp[bot[j + 1]] = temp[j];

                        bot[temp[j]] = bot[j + 1];

                        temp[j] = j + 1;
                        
                        bot[j + 1] = j;
                    }

                    if (j != temp[j] && _rnd.NextDouble() < 0.5)
                    {
                        temp[bot[j]] = temp[j];

                        bot[temp[j]] = bot[j];

                        temp[j] = j;

                        bot[j] = j;
                    } 
                    else
                        Maze[i][j].Bottom = false;
                }
            }

            for (int j = 0; j < Width; j++)
            {
                if (j != Width - 1 && j + 1 != temp[j] && (j == temp[j] || _rnd.NextDouble() < 0.5))
                {
                    Maze.Last()[j].Right = false;

                    temp[bot[j + 1]] = temp[j];

                    bot[temp[j]] = bot[j + 1];

                    temp[j] = j + 1;

                    bot[j + 1] = j;
                }
                temp[bot[j]] = temp[j];

                bot[temp[j]] = bot[j];

                temp[j] = j;

                bot[j] = j;
            }
        }

        public void PrintWithOutNumbers(int start, int h, Cell[][] maze)
        {
            for (int i = 0; i < maze[0].Length; i++)
            {
                Console.Write($"__");
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(".");
                Console.ForegroundColor = color;
            }
            Console.WriteLine();
            for (int i = start; i < h; i++)
            {
                for (int j = 0; j < maze[0].Length; j++)
                {
                    if (maze[i][j].Bottom)
                    {
                        Console.Write("__");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    if (maze[i][j].Right)
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
    }
}
