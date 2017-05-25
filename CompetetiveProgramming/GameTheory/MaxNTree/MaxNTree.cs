using System;
using System.Collections.Generic;
using System.Linq;
using CompetetiveProgramming.TimeManagement;

namespace CompetetiveProgramming.GameTheory.MaxNTree {
    /**
     * @author Manwe
     *
     *         MaxNTree class allows to find the best move a player can do
     *         considering the other N players will be playing their best move at
     *         each iteration It's algorithm is quite simple, it explores the game
     *         tree applying and canceling all the possible moves of each player
     *         successively When reaching the fixed depth, evaluate the board. Then
     *         it back propagate the best move considering at each game tree node
     *         that the player will play its best promising move
     * 
     * Hint: If you are in pure zero sum 2 player games you should have a
     *         look to Minimax implementation 
     * Hint: You might want to use MaxN tree
     *         only considering your current player and exploring the possible moves
     *         without taking into account the others
     *
     * @param <M>
     *            The class that model a move in the game tree
     * @param <G>
     *            The class that model the Game state
     */

    public class MaxNTree<G, M> where M : class, IMove<G> where G : IGame {

        class EvaluatedMove {
            public double[] Score { get; private set; }
            public M Move { get; private set; }
            public G Game { get; private set; }

            public EvaluatedMove(double[] evaluation, M move, G game) {
                this.Score = evaluation;
                this.Move = move;
                this.Game = game;
            }
        }

        class EvaluatedMoveSorter : IComparer<EvaluatedMove> {

            private int playerId;
            private IScoreConverter converter;

            public EvaluatedMoveSorter(IScoreConverter converter) {
                this.converter = converter;
            }

            public EvaluatedMove Best(IList<EvaluatedMove> moves, int playerId) {
                this.playerId = playerId;
                return moves.OrderBy(e => e, this).First();
            }

            public int Compare(EvaluatedMove o1, EvaluatedMove o2) {
                return -System.Math.Sign(converter.Convert(o1.Score, playerId) - converter.Convert(o2.Score, playerId));
            }
        }

        private IMoveGenerator<G, M> generator;

        private EvaluatedMoveSorter sorter;

        private IScoreConverter converter;

        private Timer timer;

        private EvaluatedMove Best;

        /// <summary>
        /// Creates a new Max-N tree.
        /// </summary>
        /// <param name="timer">timer instance in order to cancel the search of the best move
        ///           if we are running out of time</param>
        /// <param name="converter">A score converter is used so we can configure how the players
        ///           are taking into consideration other players scores.</param>
        public MaxNTree(Timer timer, IScoreConverter converter) {
            this.sorter = new EvaluatedMoveSorter(converter);
            this.converter = converter;
            this.timer = timer;
        }

        /// <summary>
        /// return the best game state corresponding to the best move returned by
        /// best method It is mandatory to run best method first!
        /// </summary>
        public G bestGame() {
            return Best.Game;
        }

        /// <summary>
        /// Find the best move for the current conditions
        /// </summary>
        /// <param name="game">The current state of the game</param>
        /// <param name="generator">The move generator that will generate all the possible move of
        ///           the playing player at each turn</param>
        /// <param name="depth">the fixed depth up to which the game tree will be expanded</param>
        /// <returns>the best move you can play considering all players are selecting
        ///        the best move for them</returns>
        public M best(G game, IMoveGenerator<G, M> generator, int depth) {
            this.generator = generator;
            this.Best = bestInternal(depth, game);
            return Best.Move;
        }

        private EvaluatedMove bestInternal(int depth, G board) {
            var generatedMoves = generator.GenerateMoves(board);
            if (generatedMoves.Count > 0) {
                var evaluatedMoves = evaluatesMoves(generatedMoves, board, depth);
                var bestMove = sorter.Best(evaluatedMoves, board.CurrentPlayer());
                //if (Constants.TRACES) {
                //	Console.Error.WriteLine("Evaluated moves at depth " + depth + ": " + evaluatedMoves);
                //}
                return bestMove;
            }
            // Final state?
            return new EvaluatedMove(board.Evaluate(depth), default(M), board);
        }

        private List<EvaluatedMove> evaluatesMoves(IList<M> generatedMoves, G board, int depth) {
            var evaluatedMoves = new List<EvaluatedMove>();

            foreach (M move in generatedMoves) {
                timer.timeCheck();
                board = move.Execute(board);

                if (depth == 0) {
                    evaluatedMoves.Add(new EvaluatedMove(board.Evaluate(depth), move, board));
                }
                else {
                    var bestSubTree = bestInternal(depth - 1, board);
                    evaluatedMoves.Add(new EvaluatedMove(bestSubTree.Score, move, bestSubTree.Game));
                }

                board = move.Cancel(board);
            }

            return evaluatedMoves;
        }
    }
}
