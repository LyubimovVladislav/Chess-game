using System;

namespace Chess.Pieces
{
    public struct Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static Point Empty { get => new Point(0, 0); }

        public override bool Equals(object obj) => obj is Point other && this.Equals(other);

        public bool Equals(Point p) => X == p.X && Y == p.Y;

        public override int GetHashCode() => (X, Y).GetHashCode();

        public static bool operator ==(Point lhs, Point rhs) => lhs.Equals(rhs);

        public static bool operator !=(Point lhs, Point rhs) => !(lhs == rhs);


        public Point(int value1, int value2)
        {
            this.X = value1;
            this.Y = value2;
        }
    }
}