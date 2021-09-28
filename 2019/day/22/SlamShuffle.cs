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

            long deckSize = 119315717514047;
            long index = 2020;

            var indicies = new HashSet<long>();

            for (long i = 0; i < 101741582076661L; i++) {
                var prevIndex = index;
                foreach (var shuffleTask in shuffleTasks.Reverse()) {
                    index = shuffleTask.UnshuffleIndex(index, deckSize);
                }

                if (!indicies.Add(index)) {
                    Console.WriteLine(i);
                    break;
                }

                var diff = index - prevIndex;
                //Console.WriteLine("{0,16} {1,16} {2,16} {3,16}", prevIndex, index, diff, MathExtensions.PositiveModulo(diff, deckSize));
            }



            
            /*
            var shuffleIndicies = new List<long>();
            shuffleIndicies.Add(index);

            foreach (var shuffleTask in shuffleTasks) {
                index = shuffleTask.ShuffleIndex(index, deckSize);
                shuffleIndicies.Add(index);
            }
            
            var unshuffleIndicies = new List<long>();
            unshuffleIndicies.Insert(0, index);
            
            foreach (var shuffleTask in shuffleTasks.Reverse()) {
                index = shuffleTask.UnshuffleIndex(index, deckSize);
                unshuffleIndicies.Insert(0, index);
            }

            var str = String.Join(
                "\n",
                Enumerable.Range(0, shuffleIndicies.Count)
                .Select(i => String.Format("{0,3}: {1,16} - {2,16}", i, shuffleIndicies[i], unshuffleIndicies[i]))
                .Intertwine(shuffleTasks.Select(shuffleTask => "\t\t\t" + shuffleTask.ToString()))
            );

            Console.WriteLine(str);
            */
        }
    }
}
