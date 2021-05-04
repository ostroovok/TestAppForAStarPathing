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
        public void StartGenerate()
        {
            while (true)
            {
                lock(this)
                    _maze.Generate();
                Thread.Sleep(1000);
            }     
        }
        public List<Cell[]> MazeList { get => _maze.Maze; }
    }
}
