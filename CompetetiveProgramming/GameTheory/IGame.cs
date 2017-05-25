
namespace CompetetiveProgramming.GameTheory {
    public interface IGame {

        /// <summary>
        /// The index of the player in the evaluated array
        /// </summary>
        /// <returns>The index of the player in the evaluated array</returns>
        int CurrentPlayer();
        /// <summary>
        /// Evaluate the game for each player and score it. This is a key piece of your IA efficiency!
        /// </summary>
        /// <param name="depth">the current depth when exploring the game tree.</param>
        /// <returns>the array of evaluation for each player</returns>
        double[] Evaluate(int depth);
    }
}
