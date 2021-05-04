using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllerAlg
{
    public interface IMaze
    {
        void Generate();
        List<Cell[]> Maze { get; set; }
    }
}
