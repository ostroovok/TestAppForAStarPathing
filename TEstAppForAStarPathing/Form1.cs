using AStar;
using Cells;
using EllerAlg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEstAppForAStarPathing
{
    public partial class Form1 : Form
    {
        private Task[] _tasks;
        
        private Maze _maze;
        private AStarSearch _aStar;
        private Cell[] _path;
        private Grid _grid;
        private Vector2Int _lastPoint;

        private Bitmap _myBitmap;
        private bool _paint;

        public Form1()
        {
            InitializeComponent();
            panel1.Paint += panel1_Paint;
            pictureBox1.Size = new Size(pictureBox1.Width, pictureBox1.Height * 10);
            _paint = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            if (_maze == null || _maze.MazeList.Count == 0)
            {
                return;
            }

            var scalar = (int)numericUpDown4.Value;

            label6.Text = "Количество\n ячеек: " + _maze.MazeList.Count.ToString();
            label7.Text = "Колчиество\n closed-ячеек: " + AStarSearch.Closed.ToString();

            using (Graphics g = Graphics.FromImage(_myBitmap))
            {

                if (_maze.MazeList.Last()[0].Location.Y * scalar > pictureBox1.Size.Height - 2 * scalar)
                {
                    PanelResize();
                    PaintMaze(g, scalar);
                }
                else
                {
                    if (_maze.MazeList.Count == 1)
                    {
                        for (int j = 0; j < _maze.MazeList[0].Length; j++)
                        {
                            g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList[0][j].Location.X * scalar,
                                _maze.MazeList[0][j].Location.Y * scalar,
                                _maze.MazeList[0][j].Location.X * scalar + scalar, _maze.MazeList[0][j].Location.Y * scalar);
                        }
                    }
                    for (int j = 0; j < _maze.MazeList[0].Length; j++)
                    {
                        if (_maze.MazeList.Last()[j].Right)
                        {
                            g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList.Last()[j].Location.X * scalar + scalar,
                                _maze.MazeList.Last()[j].Location.Y * scalar,
                                _maze.MazeList.Last()[j].Location.X * scalar + scalar, _maze.MazeList.Last()[j].Location.Y * scalar + scalar);
                        }
                        if(j == 0)
                        {
                            g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList.Last()[0].Location.X * scalar,
                                _maze.MazeList.Last()[0].Location.Y * scalar,
                                _maze.MazeList.Last()[0].Location.X * scalar, _maze.MazeList.Last()[0].Location.Y * scalar + scalar);
                        }
                        if (_maze.MazeList.Last()[j].Bottom)
                        {
                            g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList.Last()[j].Location.X * scalar,
                                _maze.MazeList.Last()[j].Location.Y * scalar + scalar,
                                _maze.MazeList.Last()[j].Location.X * scalar + scalar, _maze.MazeList.Last()[j].Location.Y * scalar + scalar);
                        }
                            
                    }
                }
                RenderPath(_path, scalar, g);
            }

            pictureBox1.Image = _myBitmap;
        }

        private void PaintMaze(Graphics g, int scalar)
        {
            for (int i = 0; i < _maze.MazeList.Count; i++)
            {
                for (int j = 0; j < _maze.MazeList[0].Length; j++)
                {
                    if (_maze.MazeList[i][j].Right)
                    {
                        g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList[i][j].Location.X * scalar + scalar,
                            _maze.MazeList[i][j].Location.Y * scalar,
                            _maze.MazeList[i][j].Location.X * scalar + scalar, _maze.MazeList[i][j].Location.Y * scalar + scalar);
                    }

                    if (_maze.MazeList[i][j].Bottom)
                    {
                        g.DrawLine(new Pen(new SolidBrush(Color.Black)), _maze.MazeList[i][j].Location.X * scalar,
                            _maze.MazeList[i][j].Location.Y * scalar + scalar,
                            _maze.MazeList[i][j].Location.X * scalar + scalar, _maze.MazeList[i][j].Location.Y * scalar + scalar);
                    }
                }
            }
            RenderPath(_path, scalar, g);
        }

        private void PanelResize()
        {
            pictureBox1.Size = new Size(pictureBox1.Size.Width, pictureBox1.Size.Height * 2);
            _myBitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height * 2);
        }

        private void PanelRepaint()
        {
            while (_paint)
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

                _tasks = new Task[3];

                _myBitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);

                _maze = new Maze((int)numericUpDown1.Value);

                _lastPoint = new Vector2Int((int)numericUpDown2.Value, (int)numericUpDown3.Value);

                _tasks[0] = Task.Factory.StartNew(() =>
                    _maze.StartGenerate());

                _tasks[1] = Task.Factory.StartNew(() =>
                    AStarSearchSteps());

                _tasks[2] = Task.Factory.StartNew(() =>
                    PanelRepaint());

                _paint = true;

            }
            else
            {
                _maze.Stop();
                _aStar = null;
                AStarSearch.NullifyClosedCount();
                _paint = false;
                _tasks = null;
            }
        }

        private void AStarSearchSteps()
        {
            while (_paint)
            {
                lock (_maze)
                {
                    if (_maze.MazeList.Count > 4)
                    {
                        _grid = new(_maze.MazeList);
                        _aStar = new AStarSearch(_grid);
                        _path = _aStar.Find(_lastPoint);
                        _lastPoint = _path?.Last().Location ?? new Vector2Int(0, 0);
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void RenderPath(IList<Cell> path, int scalar, Graphics g)
        {
            if(path == null)
            {
                return;
            }
            PointF[] pathh = path.Select(n => new PointF(n.Location.X * scalar + scalar / 2, n.Location.Y * scalar + scalar / 2)).ToArray();

            if (pathh.Count() > 1)
            {
                g.DrawLines(new Pen(new SolidBrush(Color.Blue), scalar / 8), pathh);
            }
            if(pathh.Count() >= 1)
            {
                CircleAtPoint(g, pathh.First(), scalar/ 10, Color.Red);
            }
        }
        private void CircleAtPoint(Graphics graphics, PointF center, float radius, Color color)
        {
            var shifted = new RectangleF(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            graphics.FillEllipse(new SolidBrush(color), shifted);
        }
    }
}

