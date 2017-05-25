using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetetiveProgramming.Genetic;
using System.Diagnostics;

namespace CompetetiveProgrammingTests {
    [TestClass]
    public class GeneticTest {
        class GAGenerator : ICandidateGenerator<int[]> {
            private Random rnd;
            public GAGenerator(Random rnd) {
                this.rnd = rnd;
            }
            public int[] generateRandomly() {
                return new int[] { rnd.Next(10), rnd.Next(10), rnd.Next(10), rnd.Next(10) };
            }
        }
        class GAMerger : ICandidateMerger<int[]> {
            private Random rnd;
            public GAMerger(Random rnd) {
                this.rnd = rnd;
            }
            public int[] merge(int[] first, int[] other) {
                int pos = rnd.Next(first.Length - 1) + 1;
                int[] ret = new int[first.Length];
                for (int i = 0; i < pos; i++) {
                    ret[i] = first[i];
                }
                for (int i = pos; i < first.Length; i++) {
                    ret[i] = other[i];
                }
                return ret;
            }
        }
        class GAMutator : ICandidateMutator<int[]> {
            private Random rnd;
            public GAMutator(Random rnd) {
                this.rnd = rnd;
            }
            public int[] mutate(int[] candidate) {
                int pos = rnd.Next(candidate.Length);
                int val = rnd.NextDouble() < .5 ? 1 : -1;
                int[] ret = new int[candidate.Length];
                Array.Copy(candidate, ret, candidate.Length);
                ret[pos] += val;
                return ret;
            }
        }
        class GAFitnessFunction : IFitnessFunction<int[]> {
            private int[] toBeFound;
            public GAFitnessFunction(int[] find) {
                toBeFound = find;
            }
            public double evaluate(int[] genotype) {
                double result = 0;
                for (int i = 0; i < genotype.Length; i++) {
                    result += Math.Abs(genotype[i] - toBeFound[i]) >> 1;
                }
                return result;
            }
        }
        private class Combination {
            public static Combination newInstance() {
                generatorValue = (generatorValue + 1) % 9;
                return new Combination(generatorValue, generatorValue, generatorValue, generatorValue);
            }

            public int first;
            public int second;
            public int third;
            public int fourth;
            private static int generatorValue = 0;

            public Combination(int first, int second, int third, int fourth) {
                this.first = first;
                this.second = second;
                this.third = third;
                this.fourth = fourth;
            }

            public override bool Equals(Object obj) {
                if (!(obj is Combination)) {
                    return false;
                }
                Combination other = (Combination)obj;
                if (first != other.first) {
                    return false;
                }
                if (fourth != other.fourth) {
                    return false;
                }
                if (second != other.second) {
                    return false;
                }
                if (third != other.third) {
                    return false;
                }
                return true;
            }

            public override int GetHashCode() {
                int prime = 31;
                int result = 1;
                result = prime * result + first;
                result = prime * result + fourth;
                result = prime * result + second;
                result = prime * result + third;
                return result;
            }

            public static bool randombool() {
                return random.NextDouble() < .5;
            }

            public override string ToString() {
                return "C[" + first + "" + second + "" + third + "" + fourth + "]";
            }
        }

        private static Random random = new Random(0);

        //Random stuff are not easy to test :D
        //Combination could be only found with combination (generator generates only identical values) and mutations (9 could not be reached)
        [TestMethod]
        public void testCombinationGuesser() {
            int[] toBeFound = new int[] { 0, 3, 7, 9 };
            GeneticAlgorithm<int[]> algo = new GeneticAlgorithm<int[]>(
                new GAFitnessFunction(toBeFound),
                new GAGenerator(random),
                new GAMerger(random),
                new GAMutator(random));
            Stopwatch sw = new Stopwatch();
            sw.Start();

            algo.Initialize(9);
            int i = 0;
            do {
                i++;
                algo.Iterate(1, 5, 20, 20, 20);
            } while (sw.ElapsedMilliseconds < 90);
            //algo.printTo(System.err);
            sw.Stop();
            Assert.AreEqual(toBeFound, algo.best());
        }
    }
}