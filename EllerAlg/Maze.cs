using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EllerAlg
{
    public class Maze
    {
         
        private IMaze _maze;
        private bool _stop = true;
        public Maze(int width)
        {
            _maze = new MazeGenerator(width);
        }

        public Maze(int width, int height)
        {
            _maze = new MazeCreator(width, height);
        }

        public Cell[] Last() => _maze.Maze.Last();
        public void Create()
        {
            _maze.Generate();
        }
        public void Stop()
        {
            _stop = false;
            _maze.Clear();
        }
        public void StartGenerate()
        {
            while (_stop)
            {
                lock(this)
                    _maze.Generate();
                Thread.Sleep(50);
            }     
        }
        public List<Cell[]> MazeList { get => _maze.Maze; }
    }
}
