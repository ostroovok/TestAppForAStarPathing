using System;
using System.Collections.Generic;
using System.Linq;

namespace Cells
{
    public class Grid : IGridProvider
    {
        public Cell[,] Cells { get; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Cell this[int x, int y] => Cells[x, y];

        public Vector2Int Size => new Vector2Int(Width, Height);
        public Cell this[Vector2Int location] => Cells[location.Y, location.X];

        public Grid(Cell[,] cells)
        {
            Cells = cells;
            Width = cells.GetUpperBound(1) + 1;
            Height = cells.GetUpperBound(0) + 1;
        }

        public Grid(List<Cell[]> cells)
        {
            Width = cells.Last().Length;
            Height = cells.Count;
            Cells = new Cell[Height, Width];
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells[i].Length; j++)
                {
                    Cells[i, j] = new Cell(cells[i][j].Location)
                    {
                        Left = cells[i][j].Left,
                        Bottom = cells[i][j].Bottom,
                        Right = cells[i][j].Right,
                        Top = cells[i][j].Top
                    };
                }
            }
        }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            Cells = new Cell[width, height];
            Reset();
        }

        public void Reset()
        {
            for (var x = 0; x <= Cells.GetUpperBound(0); x++)
            {
                for (var y = 0; y <= Cells.GetUpperBound(1); y++)
                {

                    var cell = Cells[x, y];

                    cell.G = 0;
                    cell.H = 0;
                    cell.F = 0;
                    cell.Closed = false;
                    cell.Parent = null;
                }
            }
        }

        public void Print()
        {
            for (var x = 0; x <= Cells.GetUpperBound(0); x++)
            {
                for (var y = 0; y <= Cells.GetUpperBound(1); y++)
                {
                    //Console.Write(Cells[x, y].Blocked + ", ");
                }
                Console.WriteLine();
            }
        }

        public int GetNodeId(Vector2Int location) => location.X * Width + location.Y;
    }
}