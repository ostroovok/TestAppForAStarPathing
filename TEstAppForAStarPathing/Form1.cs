using AStar;
using Cells;
using EllerAlg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TEstAppForAStarPathing
{
    public partial class Form1 : Form
    {
        private Thread _paintThread = null;
        private Thread _mazeThread = null;
        private Thread _aStarThread = null;
        private bool _paint;
        private Maze _maze;
        private AStarSearch _aStar;
        private Cell[] _path;
        private Grid _grid;
        private Bitmap _myBitmap;
        private Vector2Int _lastPoint;
        private int _scalar;
        public Form1()
        {
            InitializeComponent();
            panel1.Paint += panel1_Paint;
            _myBitmap = new Bitmap(panel1.Width, panel1.Height);
            _paint = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (!_paint)
                return;

            _scalar = (int)numericUpDown4.Value;

            if (_maze == null || _maze.MazeList.Count == 0)
                return;

            label6.Text = _maze.MazeList.Count.ToString();
            label7.Text = AStarSearch.Closed.ToString();

            var g = Graphics.FromImage(_myBitmap);
            for (int j = 0; j < _maze.MazeList[0].Length; j++)
            {
                if (_maze.MazeList.Last()[j].Location.Y * _scalar > pictureBox1.Size.Height)
                    PanelResize(g);
                if (_maze.MazeList.Last()[j].Right)
                    g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList.Last()[j].Location.X * _scalar + _scalar,
                        _maze.MazeList.Last()[j].Location.Y * _scalar,
                        _maze.MazeList.Last()[j].Location.X * _scalar + _scalar, _maze.MazeList.Last()[j].Location.Y * _scalar + _scalar);
                if (_maze.MazeList.Last()[j].Bottom)
                    g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList.Last()[j].Location.X * _scalar,
                        _maze.MazeList.Last()[j].Location.Y * _scalar + _scalar,
                        _maze.MazeList.Last()[j].Location.X * _scalar + _scalar, _maze.MazeList.Last()[j].Location.Y * _scalar + _scalar);
            }
            if (_path != null)
                RenderPath(_path.First().Location, _lastPoint, _path, _scalar, g);

            pictureBox1.Image = _myBitmap;
        }

        private void PaintMaze(Graphics g)
        {
            for (int i = 0; i < _maze.MazeList.Count; i++)
            {
                for (int j = 0; j < _maze.MazeList[0].Length; j++)
                {
                    if (_maze.MazeList[i][j].Right)
                        g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList[i][j].Location.X * _scalar + _scalar,
                            _maze.MazeList[i][j].Location.Y * _scalar,
                            _maze.MazeList[i][j].Location.X * _scalar + _scalar, _maze.MazeList[i][j].Location.Y * _scalar + _scalar);
                    if (_maze.MazeList[i][j].Bottom)
                        g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList[i][j].Location.X * _scalar,
                            _maze.MazeList[i][j].Location.Y * _scalar + _scalar,
                            _maze.MazeList[i][j].Location.X * _scalar + _scalar, _maze.MazeList[i][j].Location.Y * _scalar + _scalar);
                }
            }
        }

        private void PanelResize(Graphics g)
        {
            pictureBox1.Size = new Size(pictureBox1.Size.Width - 50, pictureBox1.Size.Height * 2);
            _myBitmap = new Bitmap(pictureBox1.Size.Width - 50, pictureBox1.Size.Height * 2);
            PaintMaze(g);
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
            if (!_paint)
            {
                pictureBox1.Image = null;
                _myBitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);

                _maze = new Maze((int)numericUpDown1.Value);

                _aStarThread = new(AStarSearchSteps);
                _mazeThread = new Thread(_maze.StartGenerate);
                _paintThread = new Thread(PanelRepaint);
                _lastPoint = new Vector2Int((int)numericUpDown2.Value, (int)numericUpDown3.Value);

                _mazeThread.Name = "Generator";
                _aStarThread.Name = "AStar";
                _paintThread.Name = "Painter";

                _mazeThread.Start();
                _aStarThread.Start();
                _paint = true;
                _paintThread.Start();

            }
            else
            {
                _maze.Stop();
                _aStar = null;
                AStarSearch.NullifyClosedCount();
                _paint = false;
            }
        }

        private void AStarSearchSteps()
        {
            while (true)
            {
                lock (_maze)
                {
                    if (_maze.MazeList.Count > 1)
                    {
                        _grid = new(_maze.MazeList);
                        _aStar = new AStarSearch(_grid);

                        _path = _aStar.Find(_lastPoint);
                        _lastPoint = _path?.Last().Location ?? new Vector2Int(0, 0);

                        Thread.Sleep(100);
                    }
                }

            }
        }

        private void RenderPath(Vector2Int start, Vector2Int goal, IList<Cell> path,
            int scalar, Graphics g)
        {

            PointF[] pathh = path.Select(n => new PointF(n.Location.X * scalar + scalar / 2, n.Location.Y * scalar + scalar / 2)).ToArray();

            if (pathh.Count() > 1)
                g.DrawLines(new Pen(new SolidBrush(Color.Blue), scalar / 8),
                pathh);
            CircleAtPoint(g, pathh[0], _scalar / 10, Color.Red);
        }
        private void CircleAtPoint(Graphics graphics, PointF center, float radius, Color color)
        {
            var shifted = new RectangleF(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            graphics.FillEllipse(new SolidBrush(color), shifted);
        }
    }
}

