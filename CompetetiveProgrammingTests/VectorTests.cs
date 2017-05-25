using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetetiveProgramming.Geometry;
using System.Collections.Generic;

namespace CompetetiveProgrammingTests {
    [TestClass]
    public class VectorTests {
        [TestMethod]
        public void TestMath() {
            Vector p1 = new Vector(0, 0);
            Vector p2 = new Vector(1, -1);
            Assert.IsTrue(p1 + p2 == new Vector(1, -1), "Vector addition 1 failed.");
            Assert.IsTrue(p1 - p2 == new Vector(-1, 1), "Vector subtraction 1 failed.");
            Assert.IsTrue(p2 * 3 == new Vector(3, -3), "Vector multiplication 1 failed.");
            Assert.IsTrue(new Vector(2, 3).Dot(new Vector(3, 2)) == 12, "Dot 1 failed");
            Assert.IsTrue(new Vector(2, 3).PerpDot(new Vector(3, 2)) == -5, "PerpDot 1 failed");
            //Perp dot on same line
            Assert.IsTrue(new Vector(2, 3).PerpDot(new Vector(4, 6)) == 0, "PerpDot 3 failed");
        }
        [TestMethod]
        public void TestAngleDetection() {
            //Calculates angle to a point in each cardinal direction
            var tests = new List<KeyValuePair<Vector, double>>();
            tests.Add(new KeyValuePair<Vector, double>(new Vector(1, 0), 0d));
            tests.Add(new KeyValuePair<Vector, double>(new Vector(1, 1), Math.PI / 4));
            tests.Add(new KeyValuePair<Vector, double>(new Vector(0, 1), Math.PI / 2));
            tests.Add(new KeyValuePair<Vector, double>(new Vector(-1, 1), 3 * Math.PI / 4));
            tests.Add(new KeyValuePair<Vector, double>(new Vector(-1, 0), Math.PI));
            tests.Add(new KeyValuePair<Vector, double>(new Vector(-1, -1), -3 * Math.PI / 4));
            tests.Add(new KeyValuePair<Vector, double>(new Vector(0, -1), -Math.PI / 2));
            tests.Add(new KeyValuePair<Vector, double>(new Vector(1, -1), -Math.PI / 4));
            string format = "AngleTo fails on {0}.  Expected {1}, got {2}";
            Vector origin = new Vector(0, 0);
            foreach (var test in tests) {
                double angle = origin.AngleTo(test.Key);
                Assert.IsTrue(angle == test.Value, string.Format(format, test.Value, test.Key, angle));
            }
        }
        [TestMethod]
        public void TestAngleProjection() {
            //Test GetPointAt in all 8 cardinal directions.  Origin assumed to be in top-left
            var tests = new List<KeyValuePair<double, Vector>>();
            tests.Add(new KeyValuePair<double, Vector>(0d, new Vector(1, 0)));
            tests.Add(new KeyValuePair<double, Vector>(Math.PI / 4, new Vector(1, -1)));
            tests.Add(new KeyValuePair<double, Vector>(Math.PI / 2, new Vector(0, -1)));
            tests.Add(new KeyValuePair<double, Vector>(3 * Math.PI / 4, new Vector(-1, -1)));
            tests.Add(new KeyValuePair<double, Vector>(Math.PI, new Vector(-1, 0)));
            tests.Add(new KeyValuePair<double, Vector>(-3 * Math.PI / 4, new Vector(-1, 1)));
            tests.Add(new KeyValuePair<double, Vector>(-Math.PI / 2, new Vector(0, 1)));
            tests.Add(new KeyValuePair<double, Vector>(-Math.PI / 4, new Vector(1, 1)));
            string format = "GetPointAt fails on {0}.  Expected {1}, got {2}";
            Vector origin = new Vector(0, 0);
            foreach (var pair in tests) {
                double hypotenuse = origin.Distance(pair.Value);
                Vector point = origin.GetPointAt(hypotenuse, pair.Key);
                Assert.IsTrue(point == pair.Value,
                    string.Format(format, pair.Key, pair.Value, point));
            }
        }
        [TestMethod]
        public void TestEquality() {
            Vector p1 = new Vector(0, 0);
            Vector p2 = new Vector(1, 0);
            Vector p3 = new Vector(0, 0);
            Assert.IsTrue(p1 == p3, "Equality test to same coordinates failed.");
            Assert.IsFalse(p1 == p2, "Equality test for 2 different points failed");
            Assert.IsFalse(p1 != p3, "Inequality test to same point failed.");
            Assert.IsTrue(p1 != p2, "Inequality test for 2 different points failed");
        }
        [TestMethod]
        public void TestDistance() {
            Vector p1 = new Vector(0, 0);
            Vector p2 = new Vector(3, 4);
            Assert.IsTrue(p1.Distance(p2) == 5, "Distance calc 1 failed");
            Assert.IsTrue(p1.Distance2(p2) == 25, "Distance2 calc 1 failed");
            Assert.IsTrue(new Vector(1, 1).Closest(p1, new Vector(2, 0)) == new Vector(1, 0), "Closest point 1 failed.");
            //Same test for point that is already on the line
            Assert.IsTrue(new Vector(1, 0).Closest(p1, new Vector(2, 0)) == new Vector(1, 0), "Closest point 3 failed.");
        }
    }
}
