using System;
using System.Collections.Generic;

namespace CompetetiveProgramming.Geometry {
    public struct Point {
        public int X;
        public int Y;
        public static readonly Point UP;
        public static readonly Point DOWN;
        public static readonly Point LEFT;
        public static readonly Point RIGHT;
        public static readonly Point NullPoint;
        static Point() {
            UP = new Point(0, -1);
            DOWN = new Point(0, 1);
            LEFT = new Point(-1, 0);
            RIGHT = new Point(1, 0);
            NullPoint = new Point(-1, -1);
        }
        public Point(int x, int y) {
            X = x;
            Y = y;
        }
        public Point(Point other) {
            X = other.X;
            Y = other.Y;
        }
        public Point(Vector other) {
            X = (int)(other.X);
            Y = (int)(other.Y);
        }
        public static Point operator +(Point p1, Point p2) {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static Point operator -(Point p1, Point p2) {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static bool operator ==(Point p1, Point p2) {
            return p1.X == p2.X && p1.Y == p2.Y;
        }
        public static bool operator !=(Point p1, Point p2) {
            return p1.X != p2.X || p1.Y != p2.Y;
        }
        public override string ToString() {
            return X.ToString() + " " + Y.ToString();
        }
        public override int GetHashCode() {
            return X * 20011 + Y;
        }
        public override bool Equals(object obj) {
            if (!(obj is Point))
                return false;
            Point p = (Point)obj;
            return X == p.X && Y == p.Y;
        }
        /// <summary>
        /// Manhattan distance to the other point
        /// </summary>
        /// <param name="t"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public double Manhattan(Point other) {
            return System.Math.Abs(other.X - X) + System.Math.Abs(other.Y - Y);
        }
        /// <summary>
        /// Chebyshev (Manhattan with diagonals) distance to the other point
        /// </summary>
        public double Chebyshev(Point other) {
            return System.Math.Max(System.Math.Abs(other.X - X), System.Math.Abs(other.Y - Y));
        }
        /// <summary>
        /// The list of points that lie 1 unit from this one, in the 4 cardinal directions
        /// </summary>
        /// <returns></returns>
        public List<Point> GetAdjacentPoints() {
            List<Point> ret = new List<Point>();
            ret.Add(this + DOWN);
            ret.Add(this + UP);
            ret.Add(this + LEFT);
            ret.Add(this + RIGHT);
            return ret;
        }
    }
}
