using Cells;
using System;
using System.Collections.Generic;

namespace AStar
{

    public class AStarSearch
    {

        private readonly IGridProvider _grid;
        private readonly FastPriorityQueue _open;

        public AStarSearch(IGridProvider grid)
        {

            _grid = grid;
            _open = new FastPriorityQueue(_grid.Size.X * _grid.Size.Y);
        }

        private double Heuristic(Cell cell, Cell goal)
        {

            var dX = Math.Abs(cell.Location.X - goal.Location.X);
            var dY = Math.Abs(cell.Location.Y - goal.Location.Y);

            // Octile distance
            return 1 * (dX + dY) + (Math.Sqrt(2) - 2 * 1) * Math.Min(dX, dY);
        }

        public void Reset()
        {

            _grid.Reset();
            _open.Clear();
        }

        private void CreateGoalCell(Cell startCell, out Cell goalCell)
        {
            var max = new Cell(new Vector2Int(0, 0)) { H = 1000.0 };
            for (int i = 0; i < _grid.Size.X; i++)
            {
                if (!_grid[new Vector2Int(i, _grid.Size.Y - 1)].Bottom && !_grid[new Vector2Int(i, _grid.Size.Y - 1)].Top)
                {
                    goalCell = _grid[new Vector2Int(i, _grid.Size.Y - 1)];
                    if (Heuristic(startCell, goalCell) < max.H)
                        max = goalCell;
                }
            }
            goalCell = max;
        }
        public Cell[] Find(Vector2Int start)
        {

            Reset();

            Cell startCell = _grid[start];
            //Cell goalCell;
            //CreateGoalCell(startCell, out goalCell);

            Cell[] goalCells = new Cell[_grid.Size.X];
            for (int i = 0; i < _grid.Size.X; i++)
            {
                goalCells[i] = _grid[new Vector2Int(i, _grid.Size.Y - 1)];
            }

            //if (goalCell == null)
                //return null;
               
            _open.Enqueue(startCell, 0);   // not checked

            var bounds = _grid.Size;

            Cell node = null;

            while (_open.Count > 0)
            {
                node = _open.Dequeue();

                node.Closed = true;

                var g = node.G + 1;

                foreach (var goalCell in goalCells)
                {
                    if (goalCell.Location == node.Location) 
                        return LinkNodes(node);
                }
                

                Vector2Int proposed = new(0, 0);

                for (var i = 0; i < PathingConstants.Directions.Length; i++)
                {
                    var direction = PathingConstants.Directions[i];
                    proposed.X = node.Location.X + direction.X;
                    proposed.Y = node.Location.Y + direction.Y;
                    if (proposed.X < 0 || proposed.X >= bounds.X ||
                        proposed.Y < 0 || proposed.Y >= bounds.Y)
                        continue;

                    if (node.Walls[i]) continue;

                    Cell neighbour = _grid[proposed];

                    if (_grid[neighbour.Location].Closed) continue;

                    if (!_open.Contains(neighbour))
                    {

                        neighbour.G = g;
                        neighbour.H = Heuristic(neighbour, new Cell(new Vector2Int(neighbour.Location.X, _grid.Size.Y - 1)));
                        neighbour.Parent = node;
                        // F will be set by the queue
                        _open.Enqueue(neighbour, neighbour.G + neighbour.H);

                    }
                    else if (g + neighbour.H < neighbour.F)
                    {
                        neighbour.G = g;
                        neighbour.F = neighbour.G + neighbour.H;
                        neighbour.Parent = node;
                    }
                }
            }

            return LinkNodes(node);
        }

        private Cell[] LinkNodes(Cell node)
        {
            var path = new Stack<Cell>();

            while (node != null)
            {
                path.Push(node);
                node = node.Parent;
            }

            return path.ToArray();
        }
    }

}