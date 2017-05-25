using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetetiveProgramming.Geometry;

namespace CompetetiveProgrammingTests {
    [TestClass]
    public class PointTest {

    [TestMethod]
    public void add() {
        Point Point = new Point(3, 5);
        Assert.AreEqual(new Point(5, 4), Point + new Point(2, -1));
    }

    [TestMethod]
    public void distances() {
        Point Point = new Point(3, 5);

        Assert.AreEqual(0, Point.Manhattan(Point));
        Assert.AreEqual(8, Point.Manhattan(new Point(1, -1)));
        Assert.AreEqual(4, Point.Chebyshev(new Point(-1, 2)), 0.001);
    }

    [TestMethod]
    public void minus() {
        Point Point = new Point(3, 5);

        Assert.AreEqual(new Point(1, 6), Point - new Point(2, -1));
    }

    [TestMethod]
    public void convertFromVector() {
        Assert.AreEqual(new Point(1, 2), new Point(new Vector(1.2, 2.999)));
    }
}
}
