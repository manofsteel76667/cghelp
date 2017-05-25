using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CompetetiveProgramming.Genetic {
    /// <summary>
    /// /**
    /// @author Manwe
    ///
    /// Class providing an implementation of a genetic algorithm.
    /// 
    /// see <a href="https://en.wikipedia.org/wiki/Genetic_algorithm">Genetic algorithm</a>
    ///
    /// </summary>
    /// <typeparam name="Genotype">The class representing one candidate</typeparam>
    public class GeneticAlgorithm<Genotype> {
        //protected to be not random during unit testing.
        public interface IShuffler<Gtype> {
            void shuffle(List<Gtype> list);
        }
        //C# has no shuffle function so we duplicate it here
        private class DefaultShuffler : IShuffler<Genotype> {
            private Random random;
            public DefaultShuffler(Random rnd) {
                random = rnd;
            }
            public void shuffle(List<Genotype> list) {
                Random rnd = new Random();
                for (int i = list.Count - 1; i > 0; i--) {                    
                    int swap = rnd.Next(i - 1);
                    if (swap != i) {
                        Genotype hold = list[i];
                        list[i] = list[swap];
                        list[swap] = hold;
                    }
                }
            }
        }

        private readonly IFitnessFunction<Genotype> fitnessFunction;
        private readonly ICandidateGenerator<Genotype> generator;
        private readonly ICandidateMerger<Genotype> merger;
        private readonly ICandidateMutator<Genotype> mutator;

        private readonly Dictionary<Genotype, Double> cachedScores = new Dictionary<Genotype, Double>();
        private readonly List<Genotype> candidates = new List<Genotype>();

        private IShuffler<Genotype> shuffler = new DefaultShuffler(new Random());

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fitnessFunction">an implementation of the fitness function in charge of evaluating genotype quality</param>
        /// <param name="generator">an implementation of a generator in charge of randomly generating new candidates</param>
        /// <param name="merger">an implementation of a merger in charge of creating a new genotype from two already existing genotype</param>
        /// <param name="mutator">an implementation of a mutator in charge of creating a new genotype from an existing genotype but modifying some characteristics</param>
        public GeneticAlgorithm(
                IFitnessFunction<Genotype> fitnessFunction, 
                ICandidateGenerator<Genotype> generator, 
                ICandidateMerger<Genotype> merger,
                ICandidateMutator<Genotype> mutator) {
            this.fitnessFunction = fitnessFunction;
            this.generator = generator;
            this.merger = merger;
            this.mutator = mutator;
        }

        private void AddRandomCandidates(int initialPoolSize) {
            for (int i = 0; i < initialPoolSize; i++) {
                candidates.Add(generator.generateRandomly());
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>The current best genotype that has been found during the iterations</returns>
        public Genotype best() {
            return candidates[0];
        }

        private Dictionary<Genotype, double> computeScores() {
            Dictionary<Genotype, double> scores = new Dictionary<Genotype, double>();

            foreach (Genotype candidate in candidates) {
                if (cachedScores.ContainsKey(candidate)) {
                    scores.Add(candidate, cachedScores[candidate]);
                }
                else {
                    double score = fitnessFunction.evaluate(candidate);
                    scores.Add(candidate, score);
                    cachedScores.Add(candidate, score);
                }
            }

            return scores;
        }

        private void dropUnselected(int selectionNumber) {
            var retained = candidates.Take(selectionNumber).ToList();
            candidates.Clear();
            candidates.AddRange(retained);
        }

        /// <summary>
        /// Clear the candidate list and use the generator to generate a fixed number of genotypes
        /// Do not use it between iterations, or you will lose all the previous iterations results!
        /// </summary>
        /// <param name="initialPoolSize">the number of genotype to be generated</param>
        public void Initialize(int initialPoolSize) {
            candidates.Clear();
            AddRandomCandidates(initialPoolSize);
        }

        /// <summary>
        /// Performs a fixed number of iterations.
        /// Each iteration will do successively:
        ///   add iterationAdditionalRandomGenerated randomly generated candidates
        ///   generate a mergedNumber from the merge process with two candidates that has been randomly selected
        ///   mutate a mutatedNumber of randomly selected candidates
        ///   evaluate all the instances' quality and retain only the selectionNumber best
        /// </summary>
        /// <param name="numberOfIterations">the number of iterations to perform</param>
        /// <param name="iterationAdditionalRandomGenerated">the number of fully random candidate that will be generated</param>
        /// <param name="selectionNumber">the number of candidates that will be kept for the next iterations</param>
        /// <param name="mergedNumber">the number of candidate to be generated from a merge of randomly selected parents</param>
        /// <param name="mutatedNumber">the number of candidates to be generated by mutation of randomly selected candidates</param>
        public void Iterate(
            int numberOfIterations,
            int iterationAdditionalRandomGenerated,
            int selectionNumber,
            int mergedNumber,
            int mutatedNumber) {

            for (int i = 0; i < numberOfIterations; i++) {
                RunOneIteration(iterationAdditionalRandomGenerated, selectionNumber, mergedNumber, mutatedNumber);
            }
        }

        private void Merge(int mergedNumber) {
            for (int i = 0; i < mergedNumber; i++) {
                int firstIndex = (2 * i) % candidates.Count;
                int secondIndex = (2 * i + 1) % candidates.Count;
                candidates.Add(merger.merge(candidates[firstIndex], candidates[secondIndex]));
            }
        }

        private void Mutate(int mutatedNumber) {
            for (int i = 0; i < mutatedNumber; i++) {
                int index = i % candidates.Count;
                candidates.Add(mutator.mutate(candidates[index]));
            }
        }

        protected void PrintTo(StreamWriter err) {
            err.WriteLine(candidates.Count);
            err.WriteLine(candidates);
        }

        private void RemoveDuplicates() {
            HashSet<Genotype> set = new HashSet<Genotype>();
            set.UnionWith(candidates);
            candidates.Clear();
            candidates.AddRange(set);
        }

        private double RunOneIteration(
            int iterationAdditionalRandomGenerated, 
            int selectionNumber, 
            int mergedNumber, 
            int mutatedNumber) {

            AddRandomCandidates(iterationAdditionalRandomGenerated);
            Shuffle();
            Merge(mergedNumber);
            Shuffle();
            Mutate(mutatedNumber);
            RemoveDuplicates();
            var scores = computeScores();
            SortByScore(scores);
            dropUnselected(selectionNumber);
            return scores[best()];
        }

        public void SetShuffler(IShuffler<Genotype> shuffler) {
            this.shuffler = shuffler;
        }

        private void Shuffle() {

            shuffler.shuffle(candidates);
        }

        private void DefaultShuffle(List<Genotype> list) {

        }

        private void SortByScore(Dictionary<Genotype, Double> scores) {
            candidates.Sort((a, b) => scores[a].CompareTo(scores[b]));
        }

        /// <summary>
        /// Allows you to add in the candidates an already defined genotype you already know is valuable
        /// Hint: this instance might come from previous iterations and you want to continue with it
        /// </summary>
        /// <param name="reference">the instance to be added to the candidate list</param>
        public void AddReference(Genotype reference) {
            candidates.Add(reference);
        }
    }
}
