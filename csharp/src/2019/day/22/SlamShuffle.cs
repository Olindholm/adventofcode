using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {

    class SlamShuffle : AdventOfCodePuzzle {
        public SlamShuffle() : base(2019, 22) {}

        override protected void SolvePuzzle(string puzzleInput) {
            var shuffleTasks = puzzleInput.SplitToLines().Select(strLine => ShuffleTechniqueFactory.ParseShuffleTechnique(strLine));


            Console.WriteLine("The 2019 card ends up at position: {0}", WhereDoesCardXMoveTo(shuffleTasks, 10007, 2019));
            Console.WriteLine("bläh: {0}", asdf(shuffleTasks, 119315717514047L, 2020, 101741582076661L));
        }

        long WhereDoesCardXMoveTo(IEnumerable<ShuffleTechnique> shuffleTasks, long deckSize, long cardIndex) {
            var func = shuffleTasks.First().GetIndexFunction(deckSize);
            foreach (var shuffleTask in shuffleTasks.Skip(1))
                func = shuffleTask.GetIndexFunction(deckSize).Incorporate(func);

            return (long) func.Evaluate(cardIndex);
        }

        long asdf(IEnumerable<ShuffleTechnique> shuffleTasks, long deckSize, long cardIndex, long nTimes) {
            var func = shuffleTasks.First().GetIndexFunction(deckSize);
            foreach (var shuffleTask in shuffleTasks.Skip(1))
                func = shuffleTask.GetIndexFunction(deckSize).Incorporate(func);

            // Perform the function recursively, MANY TIMES
            func = func.IncorporateRecursively(nTimes);

            return (long) func.EvaluateInverse(cardIndex);
        }
    }
}
