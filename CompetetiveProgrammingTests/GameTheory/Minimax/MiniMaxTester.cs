using CompetetiveProgramming.GameTheory.MiniMax;
using CompetetiveProgramming.TimeManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompetetiveProgrammingTests.GameTheory.Minimax {
    [TestClass]
    public class MinimaxTest {
        class MiniMaxMoveEvaluator : Tester.IBestMoveEvaluator {
            Minimax<StickGame, StickMove> tree;
            public MiniMaxMoveEvaluator(Minimax<StickGame, StickMove> game) {
                this.tree = game;
            }
            public StickMove findBestMove(StickGame game, StickGenerator generator, int maxdepth) {
                return tree.best(game, new StickGenerator(), maxdepth);
            }
        }
        [TestMethod]
        public void testStickGame() {
            Timer timer = new Timer();
            Minimax<StickGame, StickMove> minimax = new Minimax<StickGame, StickMove>(timer);

            Tester.testAlgo(new MiniMaxMoveEvaluator(new Minimax<StickGame, StickMove>(timer)));
        }
    }
}
