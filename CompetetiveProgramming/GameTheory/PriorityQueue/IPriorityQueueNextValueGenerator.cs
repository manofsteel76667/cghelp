
namespace CompetetiveProgramming.GameTheory.PriorityQueue {
    public interface IPriorityQueueNextValueGenerator<T> {
        int NextValue(T newItem, T oldItem);
    }
}
