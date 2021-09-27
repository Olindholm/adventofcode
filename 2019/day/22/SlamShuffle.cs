using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    
    class SlamShuffle : AdventOfCodePuzzle {
        public SlamShuffle() : base(2019, 22) {}

        override protected void SolvePuzzle(string puzzleInput) {
            var shuffleTasks = puzzleInput.SplitToLines().Select(strLine => ShuffleTechniqueFactory.ParseShuffleTechnique(strLine));

            var newOrderedDeck = Enumerable.Range(0, 10007);

            var shuffledDeck = shuffleTasks.Aggregate(newOrderedDeck, (deck, shuffleTask) => shuffleTask.Shuffle(deck)).ToArray();
            Console.WriteLine(shuffledDeck[2019]);
        }

        int DealWithIncrement(int a) {
            return 0;
        }

        int Cut(int a) {
            return 0;
        }

        int DealIntoNewStack(int a) {
            return 0;
        }
    }
}
