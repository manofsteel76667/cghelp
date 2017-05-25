
namespace CompetetiveProgramming.Genetic {
    /// <summary>
    /// @author Manwe
    /// Interface to generate randomly a new candidate during the genetic algorithm
    /// </summary>
    /// <typeparam name="Genotype">The class representing one candidate</typeparam>
    public interface ICandidateGenerator<Genotype> {
        /// <summary>
        /// Return a new instance of Genotype that has been generated randomly
        /// </summary>
        /// <returns></returns>
        Genotype generateRandomly();
    }
}
