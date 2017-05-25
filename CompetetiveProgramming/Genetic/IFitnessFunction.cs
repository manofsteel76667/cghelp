
namespace CompetetiveProgramming.Genetic {
    /// <summary>
    /// @author Manwe
    /// Interface that evaluate a candidate. During the selection phase, only the candidates with the highest score will be retained
    /// </summary>
    /// <typeparam name="Genotype">The class representing one candidate</typeparam>
    public interface IFitnessFunction<Genotype> {
        /// <summary>
        /// </summary>
        /// <param name="genotype">the genotype to be evaluated</param>
        /// <returns>the double value representing the quality of a candidate. The higher the better.</returns>
        double evaluate(Genotype genotype);
    }
}
