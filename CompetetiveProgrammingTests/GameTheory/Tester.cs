using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompetetiveProgrammingTests.GameTheory {
    public class Tester {

    public interface IBestMoveEvaluator {
        StickMove findBestMove(StickGame game, StickGenerator generator, int maxdepth);
    }
    public static void testAlgo(IBestMoveEvaluator evaluator) {
        StickGenerator generator = new StickGenerator();

        StickGame game;
        StickMove move;

        game = new StickGame(1, 2);
        try {
			move = evaluator.findBestMove(game, generator, 1);
			Assert.IsTrue(1 == move.getSticks(), "Assert 1 failed");

	        for (int player = 0; player < 2; player++) {
	            for (int sticks = 2; sticks < 10; sticks++) {
	                for (int depth = 1; depth < 10; depth++) {
	                    game = new StickGame(player, sticks);
	                    move = evaluator.findBestMove(game, generator, depth);
	                    int sticksExpected = (sticks - 1) % 4;

	                    if (sticksExpected != 0) {//There is no solution where we can win and algo can return any move...
	                        if (sticksExpected != move.getSticks()) {
	                            Console.WriteLine("paf");
	                        }
	                        Assert.IsTrue(sticksExpected == move.getSticks(), "Assert 2 failed");
	                    }
	                    Assert.IsTrue(sticks == game.getSticksRemaining(), "Assert 3 failed");//ensure algo is restoring correctly game state
	                }
	            }
	        }
        } catch (TimeoutException) {
            Assert.Fail("Timeout.");
		}
        
    }
}
}
