using CompetetiveProgramming.TimeManagement;
using System;
using System.Collections.Generic;

namespace CompetetiveProgramming.GameTheory.MiniMax {
/// <summary>
/// author Manwe
///
///        Minimax class allows to find the best move a player can do
///        in a zero sum game considering the other player will be playing his best move at
///        each iteration.
///        It includes the alpha beta prunning optimisation in order to explore less branches.
///        It also stores the current best "killer" move in order to explore the best branches first and enhance the pruning rate
///     @see <a href="https://en.wikipedia.org/wiki/Minimax">Minimax</a> and <a href="https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning">Alpha-beta pruning</a>
/// </summary>
/// <typeparam name="G">The class that model the Game state</typeparam>
/// <typeparam name="M">The class that model a move in the game tree</typeparam>
public class Minimax<G, M> where M : class, IMove<G> where G: IGame {

    private class AlphaBetaPrunningException : Exception {
            }

    private class MinMaxEvaluatedMove : IComparable<MinMaxEvaluatedMove> {
        private readonly M move;
        private readonly double value;
        private readonly MinMaxEvaluatedMove bestSubMove;

        public MinMaxEvaluatedMove(M move, double value, MinMaxEvaluatedMove bestSubMove) {
            this.move = move;
            this.value = value;
            this.bestSubMove = bestSubMove;
        }
        public MinMaxEvaluatedMove(double value, MinMaxEvaluatedMove bestSubMove) {
            this.move = null;
            this.value = value;
            this.bestSubMove = bestSubMove;
        }

        public int CompareTo(MinMaxEvaluatedMove o) {
            if (value > o.value) {
                return 1;
            } else if (value < o.value) {
                return -1;
            }
            return 0;
        }

        public MinMaxEvaluatedMove getBestSubMove() {
            return bestSubMove;
        }

        public M getMove() {
            return move;
        }

        public double getValue() {
            return value;
        }

        public override String ToString() {
            String str = "M[" + value + ",[";
            if (move != null) {
                str += move.ToString() + ",";
            }
            MinMaxEvaluatedMove subMove = bestSubMove;
            while (subMove != null) {
                if (subMove.move != null) {
                    str += subMove.ToString() + ",";
                }
                subMove = subMove.bestSubMove != null ? subMove.bestSubMove : null;
            }
            return str + "]";
        }
    }

    private int depthmax;
    private MinMaxEvaluatedMove killer;

    private readonly Timer timer;

    /**
     * Minimax constructor
     * 
     * @param timer
	 *            timer instance in order to cancel the search of the best move
	 *            if we are running out of time
     */
    public Minimax(Timer timer) {
        this.timer = timer;
    }

    private List<MinMaxEvaluatedMove> evaluateSubPossibilities(G game, IMoveGenerator<G, M> generator, int depth, double alpha, double beta, bool player,
            bool alphaBetaAtThisLevel, MinMaxEvaluatedMove previousAnalysisBest) {
        List<MinMaxEvaluatedMove> moves = new List<MinMaxEvaluatedMove>();

        List<M> orderedMoves;
        var generatedMoves = generator.GenerateMoves(game);

        // killer first
        if (previousAnalysisBest != null && generatedMoves.Contains(previousAnalysisBest.getMove())) {
            orderedMoves = new List<M>();
            int killer = generatedMoves.IndexOf(previousAnalysisBest.getMove());
            orderedMoves.Add(generatedMoves[killer]);
            generatedMoves.RemoveAt(killer);
            orderedMoves.AddRange(generatedMoves);
        }
        else {
            orderedMoves = (List<M>)generatedMoves;
        }

        foreach (M move in orderedMoves) {
            timer.timeCheck();
            G movedGame = move.Execute(game);
            MinMaxEvaluatedMove child = null;
            try {
                MinMaxEvaluatedMove bestSubChild = minimax(movedGame, generator, depth - 1, alpha, beta, !player, previousAnalysisBest == null ? null
                        : previousAnalysisBest.getBestSubMove());
                child = new MinMaxEvaluatedMove(move, bestSubChild.getValue(), bestSubChild);
            } catch (AlphaBetaPrunningException) {
                move.Cancel(game);
            }
            if (child != null) {
                // Alpha beta prunning
                if (alphaBetaAtThisLevel) {
                    if (player) {
                        alpha = System.Math.Max(alpha, child.getValue());
                        if (beta <= alpha) {
                            move.Cancel(game);
                            throw new AlphaBetaPrunningException();
                        }
                    } else {
                        beta = System.Math.Min(beta, child.getValue());
                        if (beta <= alpha) {
                            move.Cancel(game);
                            throw new AlphaBetaPrunningException();
                        }
                    }
                }
                moves.Add(child);
                move.Cancel(game);
            }
        }
        return moves;
    }

    private MinMaxEvaluatedMove minimax(G game, IMoveGenerator<G, M> generator, int depth, double alpha, double beta, bool player,
            MinMaxEvaluatedMove previousAnalysisBest)  {
        if (depth == 0) {
            return new MinMaxEvaluatedMove(null, scoreFromEvaluatedGame(game.Evaluate(depth), game), null);// Evaluated game status
        }
        List<MinMaxEvaluatedMove> moves = evaluateSubPossibilities(game, generator, depth, alpha, beta, player, true, previousAnalysisBest);
        if (moves.Count > 0) {
            moves.Sort();
            /*if (depth == depthmax && Constants.TRACES) {
                System.err.println("Moves:" + moves);
            }*/
            return moves[player ? (moves.Count - 1) : 0];
        } else {
            return new MinMaxEvaluatedMove(null, scoreFromEvaluatedGame(game.Evaluate(depth), game), null);// Real end game status
        }
    }

    /// <summary>
    /// Search in the game tree the best move using minimax with alpha beta pruning
    /// </summary>
    /// <param name="game">The current state of the game</param>
    /// <param name="generator">The move generator that will generate all the possible move of
	///           the playing player at each turn</param>
    /// <param name="depthmax">the fixed depth up to which the game tree will be expanded</param>
    /// <returns>the best move you can play considering the other player is selecting
	///        the best move for him at each turn</returns>
    public M best(G game, IMoveGenerator<G, M> generator, int depthmax) {
        try {
            this.depthmax = depthmax;
            MinMaxEvaluatedMove best = minimax(game, generator, depthmax, Double.NegativeInfinity, Double.PositiveInfinity,
                    game.CurrentPlayer() == 0, killer);
            killer = best;
            return best.getMove();
        }
        catch (AlphaBetaPrunningException) {
            // Should never happen
            throw new Exception("evaluated move found with value not between + infinity and - infinity...");
        }
        catch (TimeoutException) {
            return killer.getMove();
        }
    }

    private double scoreFromEvaluatedGame(double[] scores, G game) {
        return scores[0] - scores[1];
    }
}
}
