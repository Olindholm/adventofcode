using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    class EmptyTile : MapTile { public EmptyTile() : base(' ') {} }
    class ScaffoldTile : MapTile { public ScaffoldTile() : base('#') {} }
    class VaccumRobot : MapTile {
        public VaccumRobot(char c) : base(c) {}
        public int GetCardinalDirection() {
            char[] cs = { '^', '>', 'v', '<' };
            return cs.Where(c => c == GetSymbol()).Select((c, i) => i).First();
        }
    }

    class SetAndForget : AdventOfCodePuzzle {
        public SetAndForget() : base(2019, 17) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Init computer
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            computer.AddInstruction(new IntcodeInput());
            computer.AddInstruction(new IntcodeOutput());
            computer.AddInstruction(new IntcodeJumpIfTrue());
            computer.AddInstruction(new IntcodeJumpIfFalse());
            computer.AddInstruction(new IntcodeLessThan());
            computer.AddInstruction(new IntcodeEquals());
            computer.AddInstruction(new IntcodeAdjustRelativeBase());

            // Load and run program
            computer.LoadProgram(program);
            computer.Run();

            var defaultTile = new UnknownTile();
            var tileMap = new Dictionary<Point2D, MapTile>();
            Point2D tilePos = Point2D.ORIGIN;

            while (computer.HasMoreOutput()) {
                int c = (int) computer.GetOutput();

                MapTile tile;
                     if (c == 35) tile = new ScaffoldTile();
                else if (c == 46) tile = new EmptyTile();
                else if (c == 10) {
                    tilePos = new Point2D(0, tilePos.GetY()-1);
                    continue;
                }
                else tile = new VaccumRobot((char) c);

                tileMap.Add(tilePos, tile);
                tilePos = tilePos.ShiftX(1);
            }

            Console.WriteLine("The camera image looks as:");
            Console.WriteLine(MapToString(tileMap, defaultTile, true));

            // Find all intersections
            var intersections = tileMap.Where(tile => tile.Value is ScaffoldTile).Where(tile => {
                Point2D pos = tile.Key;

                Point2D north = pos.ShiftY(1);
                Point2D east = pos.ShiftX(1);
                Point2D south = pos.ShiftY(-1);
                Point2D west = pos.ShiftX(-1);

                return tileMap.ContainsKey(north) && tileMap[north] is ScaffoldTile
                    && tileMap.ContainsKey(east) && tileMap[east] is ScaffoldTile
                    && tileMap.ContainsKey(south) && tileMap[south] is ScaffoldTile
                    && tileMap.ContainsKey(west) && tileMap[west] is ScaffoldTile
                ;
            }).Select(tile => tile.Key);

            int sumOfAligntmentParameters = intersections.Select(p => p.GetX() * -p.GetY()).Sum();
            Console.WriteLine("The sum of all the alignment parameters is: {0}", sumOfAligntmentParameters);

            // Part two
            // Find the instructions
            var instructions = new List<VacuumRobotInstruction>();

            // Find robot
            var robotTile = tileMap.Where(tile => tile.Value is VaccumRobot).First();
            int robotDir = ((VaccumRobot) robotTile.Value).GetCardinalDirection();
            var robotPos = robotTile.Key;

            while (true) {
                int[] dirs = { robotDir+1, robotDir-1 };

                foreach (var dir in dirs) {
                    int dist = 0;

                    while (true) {
                        Point2D pos = robotPos.Shift(dir, dist+1);
                        if (!(tileMap.ContainsKey(pos) && tileMap[pos] is ScaffoldTile)) break;
                        dist++;
                    }

                    if (dist > 0) {
                        // Add instruction
                        instructions.Add(new VacuumRobotInstruction(dir - robotDir, dist));

                        // Update robot
                        robotPos = robotPos.Shift(dir, dist);
                        robotDir = dir;

                        // Continue
                        goto continueOuterLoop;
                    }
                }

                // If there's no 'continue'
                // We got here, and we're at the end of the map
                // It is time to break
                break;

                // Goto marker, to be able to continue outer loop
                continueOuterLoop:;
            }

            // Print instructions
            //foreach (var instruction in instructions) Console.WriteLine(instruction);

            // Find the sub functions
            // They may be 1-3 in total
            var maxPatterns = 3;
            var maxPatternLength = 20 / 4;
            var patterns = findPatterns(instructions, maxPatterns, maxPatternLength);

            // Find pattern sequence
            var patternSequences = patterns
            .SelectMany((pattern, i) => findPatternIndex(instructions, pattern).Select(index => (i, index)))
            .OrderBy(e => e.Item2)
            .Select(e => e.Item1)
            ;

            // Change the program to force robot wake up
            program[0] = 2;

            // Reload program again
            computer.LoadProgram(program);

            // Create movement logic
            char delimiter = ',';
            char newline = '\n';
            char[] movementFunctions = { 'A', 'B', 'C' };

            // Main movement routine
            var mainMovementRoutine = PaddList(patternSequences.Select(i => (long) movementFunctions[i]), (long) delimiter);
            computer.AddInputs(mainMovementRoutine);
            computer.AddInput((long) newline);

            // Movement routines
            foreach (var pattern in patterns) {
                var movementRoutine = PaddList(pattern.SelectMany(ins => new string[] { ins.GetLetterDirection(), ins.GetSteps().ToString() }), delimiter.ToString()).SelectMany(str => str.ToCharArray()).Select(c => (long) c);
                computer.AddInputs(movementRoutine);
                computer.AddInput((long) newline);
            }

            // Run
            bool video = false;
            computer.AddInput((long) (video ? 'y' : 'n'));
            computer.AddInput((long) newline);
            computer.Run();

            long dustQuantity = 0;
            while (computer.HasMoreOutput()) dustQuantity = computer.GetOutput();

            Console.WriteLine("The amount of dust collected by the vaccum robot is: {0}", dustQuantity);
        }

        List<T> PaddList<T>(IEnumerable<T> enumerable, T padding) {
            var list = new List<T>();
            list.Add(enumerable.First());

            foreach (var e in enumerable.Skip(1)) {
                list.Add(padding);
                list.Add(e);
            }

            return list;
        }


        List<List<T>> findPatterns<T>(List<T> sequence, int maxPatterns, int maxPatternLength) {
            var sequences = new List<List<T>>();
            sequences.Add(sequence);
            return findPatterns(sequences, maxPatterns, maxPatternLength);
        }
        List<List<T>> findPatterns<T>(List<List<T>> sequences, int maxPatterns, int maxPatternLength) {

            if (maxPatterns > 0) {
                foreach (var sequence in sequences) {
                    for (int i = 0; i < sequence.Count; i++) {
                        for (int length = 0; length < Math.Min(sequence.Count-i, maxPatternLength); length++) {
                            var pattern = sequence.GetRange(i, 1+length);
                            var patternlessSequences = removePattern(sequences, pattern);

                            List<List<T>> patterns;
                            // If the patternless sequence is empty => Success!
                            if (patternlessSequences.Count == 0) patterns = new List<List<T>>();
                            // Otherwise, let's make more patterns (if possible)
                            else patterns = findPatterns(patternlessSequences, maxPatterns-1, maxPatternLength);

                            // If patterns is not null => Success!
                            if (patterns != null) {
                                patterns.Insert(0, pattern);
                                return patterns;
                            }

                            // Otherwise, tough luck, keep looking!
                        }
                    }
                }
            }

            return null;
        }

        List<int> findPatternIndex<T>(List<T> sequence, List<T> pattern) {
            var matchIndicies = new List<int>();

            for (int i = 0; i < 1 + sequence.Count - pattern.Count; i++) {

                bool match = true;
                for (int j = 0; j < pattern.Count; j++) {
                    if (!sequence[i+j].Equals(pattern[j])) {
                        match = false;
                        break;
                    }
                }

                // If we have a match!
                // Add it to matches..
                if (match) matchIndicies.Add(i);
            }

            return matchIndicies;
        }

        List<List<T>> removePattern<T>(List<List<T>> sequences, List<T> pattern) {
            var patternlessSequences = new List<List<T>>(); // Make a copy of sequences

            foreach (var sequence in sequences) {
                var matchIndicies = findPatternIndex(sequence, pattern);

                // Split and remove matches...
                matchIndicies.Add(sequence.Count);

                int index = 0;
                foreach (var matchIndex in matchIndicies) {
                    if (index < matchIndex) {
                        patternlessSequences.Add(sequence.GetRange(index, matchIndex-index));
                    }

                    index = matchIndex + pattern.Count;
                }
            }

            return patternlessSequences;
        }
    }
}
