using CompetetiveProgramming.Geometry;
using System;
using System.Collections.Generic;
using CompetetiveProgramming.Math;

namespace CompetetiveProgramming.Physics {
    /// <summary>
    /// @author Manwe
    ///
    ///  Class representing a disk. A disk is defined by a position, a speed
    ///  and a radius It contains final attributes and will thus returns
    ///  always a new instance on each operations
    ///  
    /// Anderson: This might be good as a struct in C# but then you wouldn't be able to inherit from it
    /// </summary>
    public class Disk {
        public Vector Position;
        public Vector Speed;
        public double Radius;

        /// <summary>
        /// </summary>
        /// <param name="position">the vector representing disk position</param>
        /// <param name="speed">the vector representing disk speed</param>
        /// <param name="radius">radius of the disk</param>
        public Disk(Vector position, Vector speed, double radius) {
            this.Position = position;
            this.Speed = speed;
            this.Radius = radius;
        }

        public override int GetHashCode() {
            int prime = 31;
            int result = 1;
            result = prime * result + ((Position == null) ? 0 : Position.GetHashCode());
            long temp;
            temp = BitConverter.DoubleToInt64Bits(Radius);
            result = prime * result + (int)(temp ^ (temp >> 32));
            result = prime * result + ((Speed == null) ? 0 : Speed.GetHashCode());
            return result;
        }

        public override bool Equals(Object obj) {
            if (!(obj is Disk))
                return false;
            Disk other = (Disk)obj;
            if (Position == null) {
                if (other.Position != null)
                    return false;
            }
            else if (!Position.Equals(other.Position))
                return false;
            if (BitConverter.DoubleToInt64Bits(Radius) != BitConverter.DoubleToInt64Bits(other.Radius))
                return false;
            if (Speed == null) {
                if (other.Speed != null)
                    return false;
            }
            else if (!Speed.Equals(other.Speed))
                return false;
            return true;
        }

        /// <summary>
        /// move the disk by its speed vector.
        /// </summary>
        /// <returns>a new instance of disk with the same speed but a position equals
        ///         to position + speed</returns>
        public Disk Move() {
            return new Disk(Position + Speed, Speed, Radius);
        }

        /// <summary>
        /// /**Modify the disk speed adding an acceleration vector
        /// </summary>
        /// <param name="acceleration">the acceleration vector to be added to the speed vector</param>
        /// <returns>a new instance</returns>
        public Disk Accelerate(Vector acceleration) {
            return new Disk(Position, Speed + acceleration, Radius);
        }

        /// <summary>
        /// Modify the speed of the disk by multiplying its current speed with the
        /// given factor. Hint: You might use this method to decelerate also.
        /// </summary>
        /// <param name="factor"></param>
        /// <returns>a new instance with the same radius and position but a speed
        ///         equals to speed * factor.</returns>
        public Disk Accelerate(double factor) {
            return new Disk(Position, Speed * factor, Radius);
        }

        /// <summary>
        /// identify if the disk will collide with each other assuming that both
        /// disks will remains with a constant speed. A collision occurs when the two
        /// circles touch each other
        /// </summary>
        /// <param name="other">the other disk</param>
        /// <returns>true if a collision will occurs</returns>
        public bool WillCollide(Disk other) {
            Vector toOther = other.Position - Position;
            Vector relativeSpeed = Speed - other.Speed;
            if (relativeSpeed.Length2() <= 0)// No relative movement
                return false;
            if (toOther.Dot(relativeSpeed) < 0)// Opposite directions
                return false;
            return System.Math.Abs(relativeSpeed.Norm().Ortho().Dot(toOther)) <= Radius + other.Radius;
        }

        /// <summary>
        /// returns the shortest time when the two disks will collide considering
        /// that each disk is moving at its speed vector by time unit A collision
        /// occurs when the two circles touch each other Will return Double.MAX_VALUE
        /// if no collision occurs
        /// </summary>
        /// <param name="other">the other disk</param>
        /// <returns>the time of collision Double.MAX_VALUE if no collision occurs 0
        ///        if the two disks are allready</returns>
        public double CollisionTime(Disk other) {
            Vector toOther = other.Position - Position;
            double distanceCollision = other.Radius + Radius;
            if (toOther.Length2() <= distanceCollision * distanceCollision)
                return 0;
            Vector relativeSpeed = Speed - other.Speed;

            double a = relativeSpeed.Length2();
            double b = -2 * relativeSpeed.Dot(toOther);
            double c = toOther.Length2() - distanceCollision * distanceCollision;

            List<Complex> solutions = QuadraticEquation.Solve(a, b, c);

            if (solutions.Count == 0) {
                return double.MaxValue;
            }
            if (solutions.Count == 1) {
                double solution = solutions[0].getReal();
                if (solution >= 0)
                    return solution;
                return double.MaxValue;
            }

            Complex root1 = solutions[0];
            if (!root1.isReal()) {
                return double.MaxValue;
            }
            double root1Solution = root1.getReal();
            if (root1Solution >= 0)
                return root1Solution;
            double root2Solution = solutions[1].getReal();
            if (root2Solution >= 0)
                return root2Solution;
            return double.MaxValue;
        }

        public virtual void Collide(Disk other) { }
    }
}
