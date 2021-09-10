using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using AdventOfCode;

namespace csharp {
    class Program {
        static void Main(string[] args) {
            int yearOffset = 2019;
            int dayOffset = 1;

            List<List<AdventOfCodePuzzle>> adventOfCodePuzzles = new List<List<AdventOfCodePuzzle>>();

            // 2019
            List<AdventOfCodePuzzle> adventOfCode2019Puzzles = new List<AdventOfCodePuzzle>();
            adventOfCode2019Puzzles.Add(null);
            adventOfCode2019Puzzles.Add(new ProgramAlarm1202());
            adventOfCode2019Puzzles.Add(new CrossedWires());
            adventOfCode2019Puzzles.Add(new SecureContainer());
            adventOfCode2019Puzzles.Add(new SunnyWithAChanceOfAsteroids());
            adventOfCode2019Puzzles.Add(new AmplificationCircuit());
            adventOfCode2019Puzzles.Add(new SpaceImageFormat());
            adventOfCode2019Puzzles.Add(new SensorBoost());
            adventOfCode2019Puzzles.Add(new MonitoringStation());
            adventOfCodePuzzles.Add(adventOfCode2019Puzzles);

            // Run
            int year = 0;
            int day = adventOfCodePuzzles[year].Count-1;

            AdventOfCodePuzzle puzzle = adventOfCodePuzzles[year][day];
            puzzle.Solve();
        }
    }

}
