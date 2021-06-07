using Cells;
using System.Collections.Generic;

namespace EllerAlg
{
    public interface IMaze
    {
        void Generate();
        void Clear();
        List<Cell[]> Maze { get; set; }
    }
}
