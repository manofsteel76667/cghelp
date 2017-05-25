using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetetiveProgramming.Geometry;
using System.Collections.Generic;

namespace CompetetiveProgrammingTests {
    [TestClass]
    public class PointTests {
        [TestMethod]
        public void TestMath() {
            Point p1 = new Point(0, 0);
            Point p2 = new Point(1, -1);
            Assert.IsTrue(p1 + p2 == new Point(1, -1), "Point addition failed.");
            Assert.IsTrue(p1 - p2 == new Point(-1, 1), "Point subtraction failed.");
        }
        [TestMethod]
        public void TestEquality() {
            Point p1 = new Point(0, 0);
            Point p2 = new Point(1, 0);
            Point p3 = new Point(0, 0);
            Assert.IsTrue(p1 == p3, "Equality test to same coordinates failed.");
            Assert.IsFalse(p1 == p2, "Equality test for 2 different points failed");
            Assert.IsFalse(p1 != p3, "Inequality test to same point failed.");
            Assert.IsTrue(p1 != p2, "Inequality test for 2 different points failed");
        }
        [TestMethod]
        public void TestDistance() {
            Point p1 = new Point(0, 0);
            Point p2 = new Point(3, 4);
            Assert.IsTrue(p1.Manhattan(p2) == 7, "Manhattan calc failed");
            Assert.IsTrue(p1.Chebyshev(p2) == 4, "Chebyshev calc failed");
        }
    }
}
