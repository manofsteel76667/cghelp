using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetetiveProgramming.Graph {
    /// <summary>
    /// @author Manwe
    /// 
    ///         class that models a graph and allows to scan it in order to produce
    ///         various results (just using <a
    ///         href="https://en.wikipedia.org/wiki/Breadth-first_search">BFS</a> for
    ///         now) Graph can be directed or bi directional
    /// 
    ///         Hint: you can use this graph implementation to: compute easily
    ///         distances from anywhere to one or several targets. Heat map
    ///         constructions Voronoi territory constructions etc
    ///
    ///         for optimization reasons, results are produced as an array of values
    ///         (int or double) and by convention the relation between the result and
    ///         the node is by index
    /// </summary>
    /// <typeparam name="N">The class representing a node in the graph. HashCode and equals
    ///            should be implemented correctly since Nodes will be placed in
    ///           HashMaps</typeparam>
    public class Graph<N> {
        /* 3 Convenience classes for doing breadth first search on simple graphs 
         * Would not be so necessary if anonymous classes could implement interfaces...
         */
        private class DefaultBFSTraversable<T> : IBFSTraversable<T> {
            public bool canBeVisited(T node) {
                return true;
            }
        }
        private class DefaultDoubleBFSIterator<T> : IDoubleBfsNextValueIterator<T> {
            public double nextInterationValue(double value, int iteration) {
                return iteration;
            }
        }
        private class DefaultIntBFSIterator<T> : IIntegerBfsNextValueIterator<T> {
            public int nextInterationValue(int value, int iteration) {
                return iteration;
            }
        }

        protected readonly N[] nodes;
        protected readonly List<List<int>> neighborsIndexes;
        protected bool Directed = false;

        /// <summary>
        /// Graph constructor
        /// 
        /// Conventions: the nodes you give are in an array. They are identified by
        /// their index in this array meaning that the links source and destination
        /// you provide indicates the index of the node in this array
        /// Throws ArgumentException if there is not the same number of indexes between source and
        ///             destination
        /// </summary>
        /// <param name="nodes">the array of nodes that does exists in the graph</param>
        /// <param name="linksSource">the array of index of the source nodes</param>
        /// <param name="linksDestination">the array of index of the destination nodes</param>
        /// <param name="directed">if true will consider the links you give are directed. It
        ///           means you can only go from the source node to the destination
        ///           node but not the contrary</param>
        public Graph(N[] nodes, int[] linksSource, int[] linksDestination, bool directed) {
            if (linksSource.Length != linksDestination.Length)
                throw new ArgumentException("Number of links source and destination provided does not match!");

            this.nodes = nodes;
            neighborsIndexes = new List<List<int>>();

            for (int i = 0; i < nodes.Length; i++) {
                neighborsIndexes.Add(new List<int>());
            }
            for (int i = 0; i < linksSource.Length; i++) {
                int sourceIndex = linksSource[i];
                int destinationIndex = linksDestination[i];
                createLink(sourceIndex, destinationIndex);
                if (!directed) {
                    createLink(destinationIndex, sourceIndex);
                }
                Directed = directed;
            }
        }

        protected void createLink(int sourceIndex, int destinationIndex) {
            neighborsIndexes[sourceIndex].Add(destinationIndex);
        }

        /// <summary>
        /// Breadth-first search implementation on your graph.
        /// It will iteratively:
        /// assign current level value to all source nodes
        /// scan source nodes neighbor to find the reachable nodes that have not been reached yet
        /// Compute next level value and consider all the reachable neighbors as the new source nodes
        /// 
        /// Note: sadly I did not find a way to template this method as I would do in C++ without degrading performances.
        /// So I must implement it also for int...
        /// </summary>
        /// <param name="initialValue">The value that will be assigned to all nodes before starting the BFS
        ///		Hint: giving a value impossible to reach can allow you to identify which nodes have never been reached</param>
        /// <param name="firstValue">The value that will be assigned to all the nodes in sources</param>
        /// <param name="traversable">Determines if a node should be considered in the BFS or simply ignored</param>
        /// <param name="nextValueIterator">Give the next level value from the current level value. Add 1 to compute distances</param>
        /// <param name="sources">The list of nodes that will receive the first value</param>
        /// <returns>an array with the values of each node. The index in this array correspond to the index of the node given during the constructor</returns>
        public double[] BreadthFirstSearch(double initialValue, double firstValue, IBFSTraversable<N> traversable, IDoubleBfsNextValueIterator<N> nextValueIterator,
                List<int> sources) {
            double[] results = new double[nodes.Length];
            for (int i = 0; i < results.Length; i++) results[i] = initialValue;
            //Arrays.fill(results, intialValue);
            bool[] alreadyScanned = new bool[nodes.Length];
            //for (int i = 0; i < alreadyScanned.Length; i++) alreadyScanned[i] = false;
            //Arrays.fill(alreadyScanned, false);
            var currentNodesIndex = new HashSet<int>(sources);

            iterativeDoubleBreadthFirstSearch(results, alreadyScanned, currentNodesIndex, firstValue, 0, traversable, nextValueIterator);

            return results;
        }

        private void iterativeDoubleBreadthFirstSearch(double[] results, bool[] alreadyScanned, HashSet<int> currentNodes, double value, int iteration,
                IBFSTraversable<N> traversable, IDoubleBfsNextValueIterator<N> nextValueIterator) {
            var nextNodes = new HashSet<int>();

            foreach (int index in currentNodes) {
                if (!alreadyScanned[index]) {
                    alreadyScanned[index] = true;
                    if (traversable.canBeVisited(nodes[index])) {
                        results[index] = value;
                        nextNodes.UnionWith(neighborsIndexes[index]);
                    }
                }
            }

            if (nextNodes.Count != 0) {
                iterativeDoubleBreadthFirstSearch(results, alreadyScanned, nextNodes, nextValueIterator.nextInterationValue(value, iteration + 1), iteration + 1,
                        traversable, nextValueIterator);
            }
        }

        /// <summary>
        /// Breadth-first search implementation on your graph.
        /// It will iteratively:
        /// assign current level value to all source nodes
        /// scan source nodes neighbor to find the reachable nodes that have not been reached yet
        /// Compute next level value and consider all the reachable neighbors as the new source nodes
        /// 
        /// Note: sadly I did not find a way to template this method as I would do in C++ without degrading performances.
        /// So I must implement it also for double...
        /// 
        /// Note that for compilation reasons parameters are in a different order compared to the double version
        /// </summary>
        /// <param name="initialValue">The value that will be assigned to all nodes before starting the BFS
        ///		Hint: giving a value impossible to reach can allow you to identify which nodes have never been reached</param>
        /// <param name="firstValue">The value that will be assigned to all the nodes in sources</param>
        /// <param name="traversable">Determines if a node should be considered in the BFS or simply ignored</param>
        /// <param name="nextValueIterator">Give the next level value from the current level value. Add 1 to compute distances</param>
        /// <param name="sources">The list of nodes that will receive the first value</param>
        /// <returns>an array with the values of each node. The index in this array correspond to the index of the node given during the constructor</returns>
        public int[] BreadthFirstSearch(
                int initialValue,
                IBFSTraversable<N> traversable,
                int firstValue,
                IIntegerBfsNextValueIterator<N> nextValueIterator,
                List<int> sourcesIndex) {
            int[] results = new int[nodes.Length];
            for (int i = 0; i < results.Length; i++) results[i] = initialValue;
            //Arrays.fill(results, intialValue);
            bool[] alreadyScanned = new bool[nodes.Length];
            //for (int i = 0; i < alreadyScanned.Length; i++) alreadyScanned[i] = false;
            //Arrays.fill(alreadyScanned, false);
            var currentNodesIndex = new HashSet<int>(sourcesIndex);

            iterativeIntegerBreadthFirstSearch(results, alreadyScanned, currentNodesIndex, firstValue, 0, traversable, nextValueIterator);

            return results;
        }

        private void iterativeIntegerBreadthFirstSearch(int[] results, bool[] alreadyScanned, HashSet<int> currentNodes, int value, int iteration,
                IBFSTraversable<N> traversable, IIntegerBfsNextValueIterator<N> nextValueIterator) {
            var nextNodes = new HashSet<int>();

            foreach (int index in currentNodes) {
                if (!alreadyScanned[index]) {
                    alreadyScanned[index] = true;
                    if (traversable.canBeVisited(nodes[index])) {
                        results[index] = value;
                        nextNodes.UnionWith(neighborsIndexes[index]);
                    }
                }
            }

            if (nextNodes.Count != 0) {
                iterativeIntegerBreadthFirstSearch(results, alreadyScanned, nextNodes, nextValueIterator.nextInterationValue(value, iteration + 1), iteration + 1,
                        traversable, nextValueIterator);
            }
        }
        public double[] BreadthFirstSearchDouble(double initialValue, double firstValue, List<int> sources) {
            return BreadthFirstSearch(initialValue, firstValue, new DefaultBFSTraversable<N>(), new DefaultDoubleBFSIterator<N>(), sources);
        }
        public int[] BreadthFirstSearchInt(int initialValue, int firstValue, List<int> sources) {
            return BreadthFirstSearch(initialValue, new DefaultBFSTraversable<N>(), firstValue, new DefaultIntBFSIterator<N>(), sources);
        }
        public bool BreakLink(int from, int to) {
            if (neighborsIndexes[from].Contains(to)) {
                neighborsIndexes[from].Remove(to);
                if (!Directed) {
                    neighborsIndexes[to].Remove(from);
                }
                return true;
            }
            return false;
            //Log.WriteLine("Link {0}-{1} removed.", from, to);
        }
        public void AddLink(int from, int to) {
            if (!neighborsIndexes[from].Contains(to)) {
                createLink(from, to);
            }
            if (!Directed) {
                if (!neighborsIndexes[to].Contains(from)) {
                    createLink(to, from);
                }
            }
            //Log.WriteLine("Link {0}-{1} recreated.", from, to);
        }
        public int LinkCount(int index) {
            return neighborsIndexes[index].Count;
        }
        public List<int> GetLinks(int index) {
            return neighborsIndexes[index].ToList();
        }
        public IEnumerable<N> GetNeighbors(int index) {
            foreach (int n in neighborsIndexes[index]) {
                yield return nodes[n];
            }
        }
        /// <summary>
        /// A recursive function that find articulation points using DFS
        /// </summary>
        /// <param name="u">u --> The vertex to be visited next</param>
        /// <param name="visited">visited[] --> keeps tract of visited vertices</param>
        /// <param name="disc">disc[] --> Stores discovery times of visited vertices</param>
        /// <param name="low"></param>
        /// <param name="parent">parent[] --> Stores parent vertices in DFS tree</param>
        /// <param name="ap">ap[] --> Store articulation points</param>
        void DFSArticulationPoints(int u, bool[] visited, int[] disc,
                    int[] low, int[] parent, bool[] ap, int time) {

            // Count of children in DFS Tree
            int children = 0;

            // Mark the current node as visited
            visited[u] = true;

            // Initialize discovery time and low value
            disc[u] = low[u] = ++time;

            // Go through all vertices aadjacent to this
            foreach(int v in this.neighborsIndexes[u]) {
            /*Iterator<Integer> i = adj[u].iterator();
            while (i.hasNext()) {
                int v = i.next();  // v is current adjacent of u*/

                // If v is not visited yet, then make it a child of u
                // in DFS tree and recur for it
                if (!visited[v]) {
                    children++;
                    parent[v] = u;
                    DFSArticulationPoints(v, visited, disc, low, parent, ap, time);

                    // Check if the subtree rooted with v has a connection to
                    // one of the ancestors of u
                    low[u] = System.Math.Min(low[u], low[v]);

                    // u is an articulation point in following cases

                    // (1) u is root of DFS tree and has two or more chilren.
                    if (parent[u] == -1 && children > 1)
                        ap[u] = true;

                    // (2) If u is not root and low value of one of its child
                    // is more than discovery value of u.
                    if (parent[u] != -1 && low[v] >= disc[u])
                        ap[u] = true;
                }

                // Update low value of u for parent function calls.
                else if (v != parent[u])
                    low[u] = System.Math.Min(low[u], disc[v]);
            }
        }
        /// <summary>
        /// Identifies articulation points on the graph
        /// </summary>
        /// <returns>An array of the same size as the graph indicating whether each node is an articulation point</returns>
        public bool[] ArticulationPoints() {
            // Mark all the vertices as not visited
            bool[] visited = new bool[nodes.Length];
            int[] disc = new int[nodes.Length];
            int[] low = new int[nodes.Length];
            int[] parent = new int[nodes.Length];
            bool[] ap = new bool[nodes.Length]; // To store articulation points

            // Initialize parent and visited, and ap(articulation point)
            // arrays
            for (int i = 0; i < nodes.Length; i++) {
                parent[i] = -1;
            }

            // Call the recursive helper function to find articulation
            // points in DFS tree rooted with vertex 'i'
            for (int i = 0; i < nodes.Length; i++)
                if (visited[i] == false)
                    DFSArticulationPoints(i, visited, disc, low, parent, ap, 0);

            // Now ap[] contains articulation points, print them
            /*for (int i = 0; i < nodes.Length; i++)
                if (ap[i] == true)
                    Console.Write(i + " ");*/
            return ap;
        }

        /// <summary>
        /// Ray cast in each direction, return true if the found condition is true.  Intended for use on a graph representing a rectangular coordinate grid
        /// </summary>
        /// <param name="start">Start position on the map</param>
        /// <param name="directions">Offsets to the position required to change direction during the search</param>
        /// <param name="found">Condition to look for</param>
        /// <param name="stopLooking">Condition to abandon searching in one direction.  Searching will also stop if the current position contains no link to the next one in that direction</param>
        /// <returns></returns>
        public bool RayCast(int start, int[] directions, Func<int, bool> found, Func<int, bool> stopLooking) {
            foreach (var direction in directions) {
                int tile = start;
                while (neighborsIndexes[tile].Contains(tile + direction) && !stopLooking(tile + direction)) {
                    tile += direction;
                    if (found(tile)) return true;
                }
            }
            return false;
        }
    }
}
