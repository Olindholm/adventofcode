using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    
    class SlamShuffle : AdventOfCodePuzzle {
        public SlamShuffle() : base(2019, 22) {}

        override protected void SolvePuzzle(string puzzleInput) {
            var shuffleTasks = puzzleInput.SplitToLines().Select(strLine => ShuffleTechniqueFactory.ParseShuffleTechnique(strLine));

            var deckSize = 10007;
            var index = 2019;

            foreach (var shuffleTask in shuffleTasks) {
                if (shuffleTask is DealIntoNewStackShuffle) {
                    index = deckSize-1 - index;
                }
                else if (shuffleTask is CutShuffle) {
                    int cut = ((CutShuffle) shuffleTask).GetCut();
                    index = MathExtensions.PositiveModulo(index - cut, deckSize);
                }
                else if (shuffleTask is DealWithIncrementShuffle) {
                    int increment = ((DealWithIncrementShuffle) shuffleTask).GetIncrement();
                    index = (increment * index) % deckSize;
                }
            }

            Console.WriteLine(index);
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
