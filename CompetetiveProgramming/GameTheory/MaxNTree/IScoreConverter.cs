﻿
namespace CompetetiveProgramming.GameTheory.MaxNTree {
    /**
     * @author Manwe
     * 
     * This interface allows to evaluate for a player the relative value of a game state.
     * This value will be used to determine the best move the player will select in the Max-N tree
     * This is particularly useful if you want to consider that the player is trying to maximize only its score, or its score minus the others scores or...
     */
    public interface IScoreConverter {
        double Convert(double[] rawScores, int player);
    }
}
