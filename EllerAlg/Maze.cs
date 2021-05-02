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

        private MazeGenerator _maze;
        public Maze(int width)
        {
            _maze = new MazeGenerator(width);
        }
        public Cell[] Last() => _maze.Maze.Last();

        public void StartGenerate()
        {
            while (true)
            {
                lock(_maze)
                    _maze.Generate();
                Thread.Sleep(2000);
            }     
        }
        public List<Cell[]> MazeList { get => _maze.Maze; }
    }
}
