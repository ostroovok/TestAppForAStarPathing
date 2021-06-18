using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EllerAlg
{
    public class Maze
    {

        private IMaze _maze;
        private bool _stop = false;
        public Maze(int width)
        {
            _maze = new MazeGenerator(width);
            //_maze = new ModifiedEllerMazeGenerator(width);
        }
        public void Stop()
        {
            _stop = true;
            _maze.Clear();
        }
        public void StartGenerate()
        {
            while (!_stop)
            {
                lock (this)
                {
                    _maze.Generate();
                    Thread.Sleep(300);
                }
                Thread.Sleep(50);
            }
        }

        public List<Cell[]> MazeList { get => _maze.Maze; }

    }
}
