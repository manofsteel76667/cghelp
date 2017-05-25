using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetetiveProgramming.GameTheory;

namespace CompetetiveProgrammingTests.GameTheory {
    public class StickGenerator : IMoveGenerator<StickGame, StickMove> {

        public List<StickMove> GenerateMoves(StickGame game) {
            List<StickMove> moves = new List<StickMove>();

            if (game.getSticksRemaining() > 2) {
                moves.Add(new StickMove(3));
            }
            if (game.getSticksRemaining() > 1) {
                moves.Add(new StickMove(2));
            }
            if (game.getSticksRemaining() > 0) {
                moves.Add(new StickMove(1));
            }
            return moves;
        }

    }
}
