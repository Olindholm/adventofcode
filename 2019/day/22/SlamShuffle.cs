using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    
    class SlamShuffle : AdventOfCodePuzzle {
        public SlamShuffle() : base(2019, 22) {}

        override protected void SolvePuzzle(string puzzleInput) {
            var shuffleTasks = puzzleInput.SplitToLines().Select(strLine => ShuffleTechniqueFactory.ParseShuffleTechnique(strLine));
            var numOfShuffleTasks = shuffleTasks.Count();

            Console.WriteLine("The 2019 card ends up at position: {0}", WhereDoesCardXMoveTo(shuffleTasks, 10007, 2019));
        }

        long WhereDoesCardXMoveTo(IEnumerable<ShuffleTechnique> shuffleTasks, long deckSize, long cardIndex) {
            foreach (var shuffleTask in shuffleTasks) cardIndex = shuffleTask.ShuffleIndex(cardIndex, deckSize);
            return cardIndex;
        }
    }
}
