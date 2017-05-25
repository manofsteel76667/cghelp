using System;
using System.Collections;
using System.Collections.Generic;

namespace CompetetiveProgramming.GameTheory.PriorityQueue {
    /// <summary>
    /// A priority queue implementation by BlueRaja, with some additions.  From tests it 
    /// does not appear to be as fast as the Graph
    /// class, but the numbers suggest there may be a point on large graphs where
    /// this changes.
    /// "Maybe not completely worthless"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class PriorityQueue<T> : IEnumerable<T> where T : IPriorityQueueNode {
        private int _numNodes;
        private T[] _nodes;
        private long _numNodesEverEnqueued;
        public PriorityQueue(int maxNodes) {
            _numNodes = 0;
            _nodes = new T[maxNodes + 1];
            _numNodesEverEnqueued = 0;
        }
        public int Count {
            get {
                return _numNodes;
            }
        }
        public int MaxSize {
            get {
                return _nodes.Length - 1;
            }
        }
        public void Clear() {
            Array.Clear(_nodes, 1, _numNodes);
            _numNodes = 0;
            _numNodesEverEnqueued = 0;
        }
        public bool Contains(T node) {
            if (node == null) {
                return false;
            }
            return (node.Equals(_nodes[node.QueueIndex]));
        }
        public void Enqueue(T node, double priority) {
            node.Priority = priority;
            _numNodes++;
            _nodes[_numNodes] = node;
            node.QueueIndex = _numNodes;
            node.InsertionIndex = _numNodesEverEnqueued++;
            CascadeUp(_nodes[_numNodes]);
        }
        private void Swap(T node1, T node2) {
            //Swap the nodes
            _nodes[node1.QueueIndex] = node2;
            _nodes[node2.QueueIndex] = node1;
            //Swap their indicies
            int temp = node1.QueueIndex;
            node1.QueueIndex = node2.QueueIndex;
            node2.QueueIndex = temp;
        }
        private void CascadeUp(T node) {
            //aka Heapify-up
            int parent = node.QueueIndex / 2;
            while (parent >= 1) {
                T parentNode = _nodes[parent];
                if (HasHigherPriority(parentNode, node))
                    break;
                //Node has lower priority value, so move it up the heap
                Swap(node, parentNode); //For some reason, this is faster with Swap() rather than (less..?) individual operations, like in CascadeDown()
                parent = node.QueueIndex / 2;
            }
        }
        private void CascadeDown(T node) {
            //aka Heapify-down
            T newParent;
            int finalQueueIndex = node.QueueIndex;
            while (true) {
                newParent = node;
                int childLeftIndex = 2 * finalQueueIndex;
                //Check if the left-child is higher-priority than the current node
                if (childLeftIndex > _numNodes) {
                    //This could be placed outside the loop, but then we'd have to check newParent != node twice
                    node.QueueIndex = finalQueueIndex;
                    _nodes[finalQueueIndex] = node;
                    break;
                }
                T childLeft = _nodes[childLeftIndex];
                if (HasHigherPriority(childLeft, newParent)) {
                    newParent = childLeft;
                }
                //Check if the right-child is higher-priority than either the current node or the left child
                int childRightIndex = childLeftIndex + 1;
                if (childRightIndex <= _numNodes) {
                    T childRight = _nodes[childRightIndex];
                    if (HasHigherPriority(childRight, newParent)) {
                        newParent = childRight;
                    }
                }
                //If either of the children has higher (smaller) priority, swap and continue cascading
                if (!(newParent.Equals(node))) {
                    //Move new parent to its new index.  node will be moved once, at the end
                    //Doing it this way is one less assignment operation than calling Swap()
                    _nodes[finalQueueIndex] = newParent;
                    int temp = newParent.QueueIndex;
                    newParent.QueueIndex = finalQueueIndex;
                    finalQueueIndex = temp;
                }
                else {
                    //See note above
                    node.QueueIndex = finalQueueIndex;
                    _nodes[finalQueueIndex] = node;
                    break;
                }
            }
        }
        private bool HasHigherPriority(T higher, T lower) {
            return (higher.Priority < lower.Priority ||
                (higher.Priority == lower.Priority && higher.InsertionIndex < lower.InsertionIndex));
        }
        public T Dequeue() {
            T returnMe = _nodes[1];
            Remove(returnMe);
            return returnMe;
        }
        public void Resize(int maxNodes) {
            T[] newArray = new T[maxNodes + 1];
            int highestIndexToCopy = System.Math.Min(maxNodes, _numNodes);
            for (int i = 1; i <= highestIndexToCopy; i++) {
                newArray[i] = _nodes[i];
            }
            _nodes = newArray;
        }
        public T First {
            get {
                return _nodes[1];
            }
        }
        public void UpdatePriority(T node, double priority) {
            node.Priority = priority;
            OnNodeUpdated(node);
        }
        private void OnNodeUpdated(T node) {
            //Bubble the updated node up or down as appropriate
            int parentIndex = node.QueueIndex / 2;
            T parentNode = _nodes[parentIndex];
            if (parentIndex > 0 && HasHigherPriority(node, parentNode)) {
                CascadeUp(node);
            }
            else {
                //Note that CascadeDown will be called if parentNode == node (that is, node is the root)
                CascadeDown(node);
            }
        }
        public void Remove(T node) {
            //If the node is already the last node, we can remove it immediately
            if (node.QueueIndex == _numNodes) {
                _nodes[_numNodes] = default(T);
                _numNodes--;
                return;
            }
            //Swap the node with the last node
            T formerLastNode = _nodes[_numNodes];
            Swap(node, formerLastNode);
            _nodes[_numNodes] = default(T);
            _numNodes--;
            //Now bubble formerLastNode (which is no longer the last node) up or down as appropriate
            OnNodeUpdated(formerLastNode);
        }
        public IEnumerator<T> GetEnumerator() {
            for (int i = 1; i <= _numNodes; i++)
                yield return _nodes[i];
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

}