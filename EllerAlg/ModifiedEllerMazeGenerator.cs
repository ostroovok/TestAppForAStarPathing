using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllerAlg
{
    class ModifiedEllerMazeGenerator : IMaze
    {
        private int[] _right;
        private int[] _bot;
        private int _counter = 0;

        public int Width { get; }
        private readonly int _phantomWidth;
        public List<Cell[]> Maze { get; set; }

        private Random _rnd;

        public ModifiedEllerMazeGenerator(int width)
        {
            Width = width;
            _phantomWidth = width * 2;

            _rnd = new Random();

            Maze = new List<Cell[]>();

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
            var temp = CreateOneRow(_counter, ref _right, ref _bot);

            temp[Width] = new Cell(new Vector2Int(Width, temp[_phantomWidth - Width].Location.Y))
            {
                Right = temp[Width - 1].Left,
                Left = false,
                Bottom = temp[Width - 1].Bottom,
            };

            if (temp[Width - 1].Bottom == true && temp[Width + 1].Bottom == true)
            {
                temp[Width].Bottom = true;
            }

            for (int i = Width; i <= _phantomWidth; i++)
            {   
                if(_counter % 10 == 0 && _rnd.NextDouble() > 0.8)
                {
                    temp[i] = new Cell(new Vector2Int(i, temp[_phantomWidth - i].Location.Y))
                    {
                        Right = false,
                        Left = false,
                        Bottom = true,
                    };

                    continue;
                }

                temp[i] = new Cell(new Vector2Int(i, temp[_phantomWidth - i].Location.Y))
                {
                    Right = temp[_phantomWidth - i].Left,
                    Left = temp[_phantomWidth - i].Right,
                    Bottom = temp[_phantomWidth - i].Bottom,
                };
            }

            temp.Last().Right = true;

            for (int i = 0; i < temp.Length; i++)
            {
                if (Maze.Count >= 1 && !Maze.Last()[i].Bottom) 
                {
                    temp[i].Top = false;
                }

            }
            Maze.Add(temp);
        }

        public void Clear()
        {
            Maze.Clear();
        }

        #region Private Methods
        private Cell[] CreateOneRow(int i, ref int[] right, ref int[] bot)
        {
            var temp = new Cell[_phantomWidth + 1];

            for (int c = 0; c < _phantomWidth + 1; c++)
            {
                temp[c] = new Cell(new Vector2Int(c, i));
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
                    temp[j].Bottom = false;

            }
            temp[Width - 1].Right = false;
            _counter++;
            return temp;
        }
        #endregion
    }
}
