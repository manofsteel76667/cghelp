using System.Collections.Generic;

namespace CompetetiveProgramming.GameTheory {
    public interface IMoveGenerator<G, M> where G : IGame where M : IMove<G> {
        List<M> GenerateMoves(G game);
    }
}
