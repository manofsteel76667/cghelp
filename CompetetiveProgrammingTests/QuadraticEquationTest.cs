using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetetiveProgramming.Math;
using System.Collections.Generic;

namespace CompetetiveProgrammingTests {
    [TestClass]
    public class QuadraticEquationTest {

        [TestMethod]
        public void testRealSolutions() {
            CollectionAssert.AreEqual(new List<Complex> { new Complex(1, 0) }, QuadraticEquation.Solve(1, -2, 1));
            CollectionAssert.AreEqual(new List<Complex> { new Complex(-2, 0) }, QuadraticEquation.Solve(0, 1, 2));
            //(x-2)*(x-1)=x²-3x+2
            CollectionAssert.AreEqual(new List<Complex> { new Complex(1, 0), new Complex(2, 0) }, QuadraticEquation.Solve(1, -3, 2));
            CollectionAssert.AreEqual(new List<Complex> { new Complex(1, 0), new Complex(2, 0) }, QuadraticEquation.Solve(-1, 3, -2));
        }

        [TestMethod]
        public void imaginarySolutions() {
            CollectionAssert.AreEqual(new List<Complex> { new Complex(-1, -1), new Complex(-1, 1) }, QuadraticEquation.Solve(1, 2, 2));
            CollectionAssert.AreEqual(new List<Complex> { new Complex(-1, -1), new Complex(-1, 1) }, QuadraticEquation.Solve(-1, -2, -2));
        }
    }
}
