
namespace CompetetiveProgramming.Genetic {
    /// <summary>
    /// @author Manwe
    /// Interface allowing to merge two genotype in order to build a new one combination of the two genotype characteristics
    /// </summary>
    /// <typeparam name="Genotype">The class representing one candidate</typeparam>
    
    public interface ICandidateMerger<Genotype> {
        /// <summary>
        /// </summary>
        /// <param name="first">the first instance of genotype</param>
        /// <param name="second">the second instance of genotype</param>
        /// <returns>a new instance that has been created from the first and second genotype</returns>
        Genotype merge(Genotype first, Genotype second);
    }
}
