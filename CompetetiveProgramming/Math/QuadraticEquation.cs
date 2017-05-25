﻿using System;
using System.Collections.Generic;

namespace CompetetiveProgramming.Math {
    /// <summary>
    /// @author Manwe
    ///
    /// class providing a static method to solve quadratic equations
    /// </summary>
    public class QuadraticEquation {

        /// <summary>
        ///static method solving quadratic equations:
        /// Finding x where a*x*x+b*x+c=0
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>a list of root solving the equation.
        /// The first item in the list will be the item with the lowest real part</returns>
        public static List<Complex> Solve(double a, double b, double c) {
            List<Complex> result = new List<Complex>();
            if (a == 0) {
                if (b != 0) {
                    result.Add(new Complex(-c / b, 0));
                }
                // else no solutions!
            }
            else {
                if (a < 0) {//to have smallest root first
                    a = -a;
                    b = -b;
                    c = -c;
                }
                double delta = b * b - 4 * a * c;

                if (delta < 0) {
                    double deltaSqrt = System.Math.Sqrt(-delta);

                    result.Add(new Complex(-b / (2 * a), -deltaSqrt / (2 * a)));
                    result.Add(new Complex(-b / (2 * a), deltaSqrt / (2 * a)));

                }
                else if (delta > 0) {
                    double deltaSqrt = System.Math.Sqrt(delta);

                    result.Add(new Complex((-b - deltaSqrt) / (2 * a), 0));
                    result.Add(new Complex((-b + deltaSqrt) / (2 * a), 0));
                }
                else {
                    result.Add(new Complex(-b / (2 * a), 0));
                }
            }

            return result;
        }
    }
}