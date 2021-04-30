namespace Cells
{

    public class Cell
    {
        public bool Right { get; set; } = true;
        public bool Bottom { get; set; } = true;
        public bool Left { get; set; } = true;
        public bool Top { get; set; } = true;
        public bool[] Walls { get; }
        public bool Closed { get; set; }
        public double F { get; set; }
        public double G { get; set; }
        public double H { get; set; }

        public Vector2Int Location { get; set; }

        public Cell Parent { get; set; }

        public int QueueIndex { get; set; }

        public Cell(Vector2Int location)
        {
            Walls = new[] { Left, Right, Top, Bottom };
            Location = location;
        }

        public override string ToString() => $"[{Location.X},{Location.Y}]";
    }

}