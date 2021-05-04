using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using EllerAlg;
using AStar;
using Cells;

namespace TEstAppForAStarPathing
{
    public partial class Form1 : Form
    {
        private Thread _paintThread = null;
        private Thread _mazeThread = null;
        private Thread _aStarThread = null;
        private Maze _maze;
        private AStarSearch _aStar;
        private Grid _grid;
        private Bitmap _myBitmap;
        private Vector2Int _lastPoint = new(0,0);
        public Form1()
        {
            InitializeComponent();
            panel1.Paint += panel1_Paint;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            _myBitmap = new Bitmap(panel1.Width, panel1.Height);
            var g = Graphics.FromImage(_myBitmap);

            if (_maze == null)
            {
                return;
            }
                
            foreach (var w in _maze.MazeList)
            {
                foreach (var cell in w)
                {
                    //g.FillRectangle(new SolidBrush(Color.White), cell.Location.X * 10, cell.Location.Y * 10, 10, 10);
                    if (cell.Right)
                        g.DrawLine(new Pen(new SolidBrush(Color.Black)), cell.Location.X * 10 + 10, cell.Location.Y * 10, 
                            cell.Location.X * 10 + 10, cell.Location.Y * 10 + 10);
                    if (cell.Bottom)
                        g.DrawLine(new Pen(new SolidBrush(Color.Black)), cell.Location.X * 10, cell.Location.Y * 10 + 10,
                            cell.Location.X * 10 + 10, cell.Location.Y * 10 + 10);
                }
            }
            pictureBox1.Image = _myBitmap;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(_paintThread == null)
            {
                _maze = new Maze((int)numericUpDown1.Value);
                _mazeThread = new Thread(_maze.StartGenerate);

                _grid = new(_maze.MazeList);
                _mazeThread.Start();
            }
            else
            {
                _mazeThread.Abort();
            }
            //_aStar = new AStarSearch(_grid);
            //_aStarThread = new(AStarSearchSteps);
        }

        private void AStarSearchSteps()
        {
            Random rnd = new();
            while (true)
            {
                lock (_maze)
                {
                    _grid = new(_maze.MazeList);
                    _lastPoint = _aStar.Find(_lastPoint, new Vector2Int(_grid.Height - 1,
                        rnd.Next(_grid.Width))).Last().Location;
                }
                Thread.Sleep(2000);
            }
        }
    }
}
