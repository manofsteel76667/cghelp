using CompetetiveProgramming.Graph;
using CompetetiveProgramming.TimeManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetetiveProgrammingTests {
    [TestClass]
    public class GraphTest {
        private Graph<int> graph;
        private Graph<int> directedGraph;

        public void init() {
            /*
             * 1-2-3
             * |   |
             * 4---5---6
             * |
             * 7
             * 
             * 8---9---0
             * **/
            graph = new Graph<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    new int[] { 0, 1, 1, 2, 3, 4, 4, 5, 8 },
                    new int[] { 9, 2, 4, 3, 5, 5, 7, 6, 9 },
                    false);
            /*
             * 
             * 0->1<->2<-3
             * ^         ^
             * |         |
             * 4<--------5
             * 
             * */

            directedGraph = new Graph<int>(new int[] { 0, 1, 2, 3, 4, 5 }, new int[] { 0, 1, 2, 3, 4, 5, 5 }, new int[] { 1, 2, 1, 2, 0, 3, 4 }, true);
        }
        class Not3 : IBFSTraversable<int> {

            public bool canBeVisited(int node) {
                return node != 3;
            }
        }
        class Not2 : IBFSTraversable<int> {

            public bool canBeVisited(int node) {
                return node != 2;
            }
        }
        class DoubleIterate : IDoubleBfsNextValueIterator<int> {

            public double nextInterationValue(double value, int iteration) {
                return iteration;
            }
        }
        class IntIterate : IIntegerBfsNextValueIterator<int> {

            public int nextInterationValue(int value, int iteration) {
                return iteration;
            }
        }
        void assertArrayEquals(int[] key, int[] results) {
            for (int i = 0; i < key.Length; i++) {
                Assert.AreEqual(key[i], results[i], string.Format("Int array element {0} failed", i));
            }
        }
        void assertArrayEquals(double[] key, double[] results, double threshold) {
            for (int i = 0; i < key.Length; i++) {
                Assert.IsTrue(Math.Abs(key[i] - results[i]) < threshold, string.Format("Double array element {0} failed.  Expected {1} found {2}", i, key[i], results[i]));
            }
        }
        [TestMethod]
        public void bfsOnGraph() {
            init();
            List<int> sourcesIndex = new List<int> { 0, 1, 6 };
            double[] resultsDouble = graph.BreadthFirstSearch(-1.0, 0.0, new Not3(), new DoubleIterate(), sourcesIndex);
            int[] resultsInt = graph.BreadthFirstSearch(-1, new Not3(), 0, new IntIterate(), sourcesIndex);
            assertArrayEquals(new double[] { 0, 0, 1, -1, 1, 1, 0, 2, 2, 1 }, resultsDouble, 0.001);
            assertArrayEquals(new int[] { 0, 0, 1, -1, 1, 1, 0, 2, 2, 1 }, resultsInt);

            sourcesIndex = new List<int> {4};
            resultsDouble = directedGraph.BreadthFirstSearch(-1.0, 0.0, new Not2(), new DoubleIterate(), sourcesIndex);
            resultsInt = directedGraph.BreadthFirstSearch(-1, new Not2(), 0, new IntIterate(), sourcesIndex);
            assertArrayEquals(new double[] { 1, 2, -1, -1, 0, -1 }, resultsDouble, 0.001);
            assertArrayEquals(new int[] { 1, 2, -1, -1, 0, -1 }, resultsInt);
        }
        [TestMethod]
        public void performancesBFS() {
            /*
             * computing distances in a n*n grid starting from top left angle
             * Initial timings:
             * ~750ms create graph
             * ~2800ms bfs
             * */
            int n = 500;

            Timer timer = new Timer();
            timer.startTimer(100000);

            int[] nodes = new int[n * n];
            for (int i = 0; i < n * n; i++) {
                nodes[i] = i;
            }

            int[] sources = new int[2 * n * n - 2 * n];
            int[] destinations = new int[2 * n * n - 2 * n];
            int index = 0;

            for (int i = 0; i < n * n; i++) {
                if (i % n != n - 1) {
                    //link to the right
                    sources[index] = i;
                    destinations[index] = i + 1;
                    index++;
                }
                if (i < n * n - n) {
                    //link to the bottom
                    sources[index] = i;
                    destinations[index] = i + n;
                    index++;
                }
            }

            Graph<int> grid = new Graph<int>(nodes, sources, destinations, false);


            Console.Error.WriteLine("Time taken to create the graph for a square of " + n + " :" + (timer.currentTimeTakenInTicks() / 10000 + "ms"));
            timer.startTimer(10000);
            List<int> startingNodesIndex = new List<int>();

            startingNodesIndex.Add(0);

            int[] values = grid.BreadthFirstSearchInt(0, 0, startingNodesIndex);

            int[] expected = new int[n * n];
            for (int line = 0; line < n; line++) {
                for (int column = 0; column < n; column++) {
                    expected[line + n * column] = line + column;
                }
            }

            Console.Error.WriteLine("Time taken to BFS on a square of " + n + " :" + (timer.currentTimeTakenInTicks() / 10000 + "ms"));

            assertArrayEquals(expected, values);
        }
        [TestMethod]
        public void TestExtensions() {
            init();
            var list = graph.GetLinks(0);
            Assert.IsTrue(graph.GetLinks(0).Count == 1);
            list.RemoveAt(0);
            //Test that altering the link list does not alter the internal list
            Assert.IsTrue(graph.GetLinks(0).Count == 1);
            var neighbors = graph.GetNeighbors(1);
            Assert.IsTrue(neighbors.Contains(2));
            Assert.IsTrue(neighbors.Contains(4));
            Assert.IsTrue(neighbors.Count() == 2);
        }
        [TestMethod]
        public void TestArticulationPoints() {
            init();
            var points = graph.ArticulationPoints();
            Assert.IsTrue(!points[0]);
            Assert.IsTrue(!points[1]);
            Assert.IsTrue(!points[2]);
            Assert.IsTrue(!points[3]);
            Assert.IsTrue(points[4]);
            Assert.IsTrue(points[5]);
            Assert.IsTrue(!points[6]);
            Assert.IsTrue(!points[7]);
            Assert.IsTrue(!points[8]);
            Assert.IsTrue(points[9]);
        }
    }
}

