using System;

namespace CompetetiveProgramming.Math {
    /// <summary>
    /// @author Manwe
    /// Class representing a complex number.
    /// This complex number has a real and imaginary part
    /// </summary>
    public struct Complex {
        private readonly double real;
        private readonly double imaginary;
        /// <summary>
        /// Construct a complex from a real and imaginary value
        /// </summary>
        /// <param name="real">assigned to the real part of the complex number</param>
        /// <param name="imaginary">assigned to the imaginary part of the complex number</param>
        public Complex(double real, double imaginary) {
            this.real = real;
            this.imaginary = imaginary;
        }
        public double getReal() {
            return real;
        }
        public double getImaginary() {
            return imaginary;
        }
        /// <summary>
        /// </summary>
        /// <returns>true if the complex number has no imaginary value (equals 0)</returns>
        public bool isReal() {
            return imaginary == 0;
        }
        public override string ToString() {
            return "C[" + real + ", " + imaginary + "i]";
        }
        public override int GetHashCode() {
            int prime = 31;
            int result = 1;
            long temp;
            temp = BitConverter.DoubleToInt64Bits(imaginary);
            result = prime * result + (int)(temp ^ (temp >> 32));
            temp = BitConverter.DoubleToInt64Bits(real);
            result = prime * result + (int)(temp ^ (temp >> 32));
            return result;
        }
        public override bool Equals(Object obj) {
            if (!(obj is Complex))
                return false;
            Complex other = (Complex)obj;
            if (BitConverter.DoubleToInt64Bits(imaginary) != BitConverter.DoubleToInt64Bits(other.imaginary))
                return false;
            if (BitConverter.DoubleToInt64Bits(real) != BitConverter.DoubleToInt64Bits(other.real))
                return false;
            return true;
        }
    }
}