using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using AdventOfCode;

namespace csharp {
    class Program {
        
        static void Main(string[] args) {
            new Program(args);
        }

        private SortedList<AdventOfCodeKey, AdventOfCodePuzzle> puzzles = new SortedList<AdventOfCodeKey, AdventOfCodePuzzle>();

        public Program(string[] args) {
            InitPuzzles();

            // Parse args
            List<AdventOfCodeKey> puzzlesToSolve = new List<AdventOfCodeKey>();
            AdventOfCodeKey latest = puzzles.Keys.Last();

            foreach (var arg in args) {
                // If --all arg, add all to queue
                if (arg.Equals("--all", StringComparison.OrdinalIgnoreCase)) {
                    foreach (var key in puzzles.Keys) puzzlesToSolve.Add(key);
                }
                // If --latest
                else if (arg.Equals("--latest", StringComparison.OrdinalIgnoreCase)) {
                    puzzlesToSolve.Add(latest);
                }
                // Otherwise...
                else {
                    // It's a day, or day/year
                    string[] subargs = arg.Split("/");
                    if (subargs.Length > 2) throw new Exception("Invalid input argument");

                    int day = Int32.Parse(subargs[0]);
                    int year = (subargs.Length > 1) ? Int32.Parse(subargs[1]) : latest.GetYear();

                    puzzlesToSolve.Add(new AdventOfCodeKey(day, year));
                }
            }

            // Run puzzle(s)
            foreach (var key in puzzlesToSolve) puzzles[key].Solve();
            
        }

        private void InitPuzzles() {
            AddPuzzle(new ProgramAlarm1202());
            AddPuzzle(new CrossedWires());
            AddPuzzle(new SecureContainer());
            AddPuzzle(new SunnyWithAChanceOfAsteroids());
            AddPuzzle(new UniversalOrbitMap());
            AddPuzzle(new AmplificationCircuit());
            AddPuzzle(new SpaceImageFormat());
            AddPuzzle(new SensorBoost());
            AddPuzzle(new MonitoringStation());
            AddPuzzle(new SpacePolice());
            AddPuzzle(new TheNBodyProblem());
            AddPuzzle(new CarePackage());
            AddPuzzle(new SpaceStoichiometry());
            AddPuzzle(new OxygenSystem());
            AddPuzzle(new FlawedFrequencyTransmission());
        }

        void AddPuzzle(AdventOfCodePuzzle puzzle) {
            AdventOfCodeKey key = puzzle.GetKey();

            // This will throw an exception in case of duplicates
            // (That's likely a error by the programmer)
            puzzles.Add(key, puzzle);
        }
    }

}
