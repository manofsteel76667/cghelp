using System;

namespace CompetetiveProgramming.Geometry {
    /// <summary>
    /// Multipurpose struct.  Can represent a vector or a point using doubles.
    /// </summary>
    public struct Vector {
        const double EQUALITY_LIMIT = .0000001;
        public double X;
        public double Y;
        public Vector(double x, double y) {
            X = x;
            Y = y;
        }
        public Vector(int x, int y) {
            X = x;
            Y = y;
        }
        public Vector(Vector other) {
            X = other.X;
            Y = other.Y;
        }
        public Vector(Point other) {
            X = other.X;
            Y = other.Y;
        }
        public override string ToString() {
            return string.Format("{0} {1}", X, Y);
        }
        public override int GetHashCode() {
            int prime = 31;
            int result = 1;
            long temp;
            temp = BitConverter.DoubleToInt64Bits(X);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(Y);
            result = prime * result + (int)(temp ^ (temp >> 32));
            return result;
        }
        public override bool Equals(object obj) {
            if (!(obj is Vector)) return false;
            Vector t = (Vector)obj;
            return System.Math.Abs(t.X - X) < EQUALITY_LIMIT && 
                System.Math.Abs(t.Y - Y) < EQUALITY_LIMIT;
        }
        public static Vector operator +(Vector p1, Vector p2) {
            return new Vector(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static Vector operator -(Vector p1, Vector p2) {
            return new Vector(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static Vector operator *(Vector p, double i) {
            return new Vector(p.X * i, p.Y * i);
        }
        public static Vector operator /(Vector p, double i) {
            return new Vector(p.X / i, p.Y / i);
        }
        public static bool operator ==(Vector p1, Vector p2) {
            return p1.Equals(p2);
        }
        public static bool operator !=(Vector p1, Vector p2) {
            return !(p1.Equals(p2));
        }
        /// <summary>
        /// Euclidean distance from this point to the other
        /// </summary>
        public double Distance(Vector other) {
            return System.Math.Sqrt((other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y));
        }
        /// <summary>
        /// Distance squared to the other point (faster than Euclidean calc)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public double Distance2(Vector other) {
            return (other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y);
        }

        /// <summary>
        /// Vector length
        /// </summary>
        /// <returns></returns>
        public double Length() {
            return System.Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// Vector length, squared
        /// </summary>
        /// <returns></returns>
        public double Length2() {
            return X * X + Y * Y;
        }

        public Vector Norm() {
            double length = Length();
            if (length > 0)
                return new Vector(X / length, Y / length);
            return new Vector(0, 0);
        }

        /// <summary>
        /// Returns a new vector orthogonal (perpendicular) to this one
        /// </summary>
        /// <returns></returns>
        public Vector Ortho() {
            return new Vector(-Y, X);
        }

        /// <summary>
        /// Angle from this point to another.  Assumes Origin (0,0) is top-left
        /// </summary>
        public double AngleTo(Vector other) {
            return System.Math.Atan2(other.Y - Y, other.X - X);
        }
        /// <summary>
        /// Dot product of this vector and the other
        /// </summary>
        public double Dot(Vector other) {
            return X * other.X + Y * other.Y;
        }
        /// <summary>
        /// Perpendicular dot product (determinant) of the vector.  
        /// Positive if the point lies counterclockwise to the vector, negative
        /// if clockwise, and 0 if it lies on the same line.
        /// </summary>
        public double PerpDot(Vector other) {
            return -Y * other.X + X * other.Y;
        }
        /// <summary>
        /// Returns the point lying at the specified angle and distance from this one.
        /// Assumes origin (0,0) is top-left
        /// </summary>
        public Vector GetPointAt(double dist, double angle) {
            return new Vector(X + dist * System.Math.Cos(angle), Y - dist * System.Math.Sin(angle));
        }
        /// <summary>
        /// On the line described by the 2 other points, return the point closest to this one.
        /// </summary>
        public Vector Closest(Vector a, Vector b) {
            double da = b.Y - a.Y;
            double db = a.X - b.X;
            double c1 = da * a.X + db * a.Y;
            double c2 = -db * this.X + da * this.Y;
            double det = da * da + db * db;
            double cx = 0;
            double cy = 0;

            if (det != 0) {
                cx = (da * c1 - db * c2) / det;
                cy = (da * c2 + db * c1) / det;
            }
            else {
                // The point is already on the line
                cx = this.X;
                cy = this.Y;
            }

            return new Vector(cx, cy);
        }
    }
}
