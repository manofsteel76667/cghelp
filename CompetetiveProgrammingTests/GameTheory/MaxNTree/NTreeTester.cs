using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetetiveProgramming.TimeManagement;
using CompetetiveProgramming.GameTheory.MaxNTree;

namespace CompetetiveProgrammingTests.GameTheory.MaxNTree {
    [TestClass]
    public class NTreeTester {
        class NTreeStickGameEvaluator : Tester.IBestMoveEvaluator {
            MaxNTree<StickGame, StickMove> tree;
            public NTreeStickGameEvaluator(MaxNTree<StickGame, StickMove> tree) {
                this.tree = tree;
            }
            public StickMove findBestMove(StickGame game, StickGenerator generator, int maxdepth) {
                return tree.best(game, generator, maxdepth);
            }
        }
        class NTreeScoreConverter : IScoreConverter {

            public double Convert(double[] rawScores, int player) {
                return rawScores[player];
            }
        }
        [TestMethod]
        public void TestNTree() {
            Timer timer = new Timer();
            MaxNTree<StickGame, StickMove> maxNTree = new MaxNTree<StickGame, StickMove>(timer, new NTreeScoreConverter());

            Tester.testAlgo(new NTreeStickGameEvaluator(maxNTree));//(game, generator, maxdepth) -> maxNTree.best(game, generator, maxdepth));
        }
    }
}
