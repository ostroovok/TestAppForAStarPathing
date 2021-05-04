namespace Cells
{

    public class Cell
    {
        public bool Right { get => Walls[1]; set => Walls[1] = value; }
        public bool Bottom { get => Walls[3]; set => Walls[3] = value; }
        public bool Left { get => Walls[0]; set => Walls[0] = value; }
        public bool Top { get => Walls[2]; set => Walls[2] = value; }
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
            Walls = new[] { true, true, true, true };
            Location = location;
        }

        public override string ToString() => $"[{Location.X},{Location.Y}]";
    }

}