using System;
using System.Collections;
using System.Collections.Generic;

namespace CompetetiveProgramming.GameTheory.PriorityQueue {
    public interface IPriorityQueueNode {
        double Priority { get; set; }
        long InsertionIndex { get; set; }
        int QueueIndex { get; set; }
    }
}
