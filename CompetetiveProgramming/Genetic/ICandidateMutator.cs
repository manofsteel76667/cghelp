
namespace CompetetiveProgramming.Genetic {
    /// <summary>
    /// @author Manwe
    /// Interface allowing to create a new instance of a genotype but that has been mutated
    /// </summary>
    /// <typeparam name="Genotype">The class representing one candidate</typeparam>
    public interface ICandidateMutator<Genotype> {
        /// <summary>
        /// </summary>
        /// <param name="candidate">the source instance that will be mutated</param>
        /// <returns>a new instance that has been created from the candidate and modified</returns>
        Genotype mutate(Genotype candidate);
    }
}
