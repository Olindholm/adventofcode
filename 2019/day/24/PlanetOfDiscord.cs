using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class PlanetOfDiscord : AdventOfCodePuzzle {
        public PlanetOfDiscord() : base(2019, 24) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Part one
            {
                // Initial state
                BugState thisState = BugState.Parse(puzzleInput);

                // Previous states
                var prevStates = new HashSet<BugState>();

                while (true) {
                    // Debug
                    // Print state
                    //Console.WriteLine(state);
                    //Console.WriteLine("");

                    // Add state to previous states
                    var duplicate = !prevStates.Add(thisState);

                    // If duplicate, we done!
                    if (duplicate) break;

                    // Simulate
                    thisState = thisState.Next();
                }

                Console.WriteLine("The biodiversity rating for the first layout that appears twice is: {0}", thisState.GetBiodiversityRating());
            }

            // Part two
            {
                PlutonianBugState state = PlutonianBugState.Parse(puzzleInput);
                for (int i = 0; i < 200; i++) state = state.NextState();

                Console.WriteLine("After 200 minutes there the number of bugs are: {0}", state.GetTotalBugCount());
            }
        }
    }
}
