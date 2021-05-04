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
        private Cell[] _path;
        private Grid _grid;
        private Bitmap _myBitmap;
        private Vector2Int _lastPoint = new(0,0);
        public Form1()
        {
            InitializeComponent();
            panel1.Paint += panel1_Paint;
            _myBitmap = new Bitmap(panel1.Width, panel1.Height);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var scalar = 30;
            var g = Graphics.FromImage(_myBitmap);

            if (_maze == null || _maze.MazeList.Count == 0)
                return;

            for (int j = 0; j < _maze.MazeList[0].Length; j++)
            {
                if (_maze.MazeList.Last()[j].Right)
                    g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList.Last()[j].Location.X * scalar + scalar,
                        _maze.MazeList.Last()[j].Location.Y * scalar,
                        _maze.MazeList.Last()[j].Location.X * scalar + scalar, _maze.MazeList.Last()[j].Location.Y * scalar + scalar);
                if (_maze.MazeList.Last()[j].Bottom)
                    g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList.Last()[j].Location.X * scalar,
                        _maze.MazeList.Last()[j].Location.Y * scalar + scalar,
                        _maze.MazeList.Last()[j].Location.X * scalar + scalar, _maze.MazeList.Last()[j].Location.Y * scalar + scalar);
            }
            if (_path != null)
                //lock(_aStar)
                    RenderPath(_path.First().Location, _lastPoint, _path, scalar, g);

            pictureBox1.Image = _myBitmap;
        }

        private void PanelRepaint()
        {
            while (true)
            {
                panel1.Invalidate();
                Thread.Sleep(50);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(_paintThread == null)
            {
                _maze = new Maze((int)numericUpDown1.Value);
                
                _aStarThread = new(AStarSearchSteps);
                _mazeThread = new Thread(_maze.StartGenerate);
                _paintThread = new Thread(PanelRepaint);

                _mazeThread.Name = "Generator";
                _aStarThread.Name = "AStar";
                _paintThread.Name = "Painter";

                _mazeThread.Start();
                _aStarThread.Start();
                _paintThread.Start();
            }
            else
            {
                _paintThread = null;
                _mazeThread = null;
                _aStarThread = null;
            }
        }

        private void AStarSearchSteps()
        {
            while (true)
            {
                lock (_maze)
                {
                    if(_maze.MazeList.Count > 1)
                    {
                        _grid = new(_maze.MazeList);
                        _aStar = new AStarSearch(_grid);
                        //lock (_aStar)
                        {
                            _path = _aStar.Find(_lastPoint);
                            _lastPoint = _path?.Last().Location ?? new Vector2Int(0,0);
                        }
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private void RenderPath(Vector2Int start, Vector2Int goal, IList<Cell> path,
            int scalar, Graphics g)
        {
            
            PointF[] pathh = path.Select(n => new PointF(n.Location.X * scalar + scalar/2, n.Location.Y * scalar + scalar / 2)).ToArray();

            if (pathh.Count() > 1)
                g.DrawLines(new Pen(new SolidBrush(Color.Blue), 4),
                pathh);

            //CircleAtPoint(g, new PointF(start.X * scalar/2, start.Y * scalar/2), 10, Color.Red);
            //CircleAtPoint(g, new PointF(goal.X * scalar/2, goal.Y * scalar/2), 10, Color.Blue);

            //pictureBox1.Image = _myBitmap;
        }
        private void CircleAtPoint(Graphics graphics, PointF center, float radius, Color color)
        {
            var shifted = new RectangleF(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            graphics.FillEllipse(new SolidBrush(color), shifted);
        }
    }
}

