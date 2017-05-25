
namespace CompetetiveProgramming.GameTheory {
    public interface IMove<G> where G : IGame {
        G Execute(G game);
        G Cancel(G game);
    }
}
