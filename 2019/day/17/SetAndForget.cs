using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    class EmptyTile : MapTile { public EmptyTile() : base(' ') {} }
    class ScaffoldTile : MapTile { public ScaffoldTile() : base('#') {} }
    
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
            var map = new Dictionary<Point2D, MapTile>();
            Point2D pos = Point2D.ORIGIN;

            while (computer.HasMoreOutput()) {
                int c = (int) computer.GetOutput();

                MapTile tile;
                     if (c == 35) tile = new ScaffoldTile();
                else if (c == 46) tile = new EmptyTile();
                else if (c == 10) {
                    pos = new Point2D(0, pos.GetY()+1);
                    continue;
                }
                else tile = new MapTile((char) c);

                if (tile == null) Console.WriteLine(c);
                if (tile == null) Console.WriteLine(pos);
                map.Add(pos, tile);
                pos = pos.ShiftX(1);
            }

            Console.WriteLine("The camera image looks as:");
            Console.WriteLine(MapToString(map, defaultTile));

            // Find all intersections
            var intersections = map.Where(tile => tile.Value is ScaffoldTile).Where(tile => {
                Point2D pos = tile.Key;

                Point2D north = pos.ShiftY(1);
                Point2D east = pos.ShiftX(1);
                Point2D south = pos.ShiftY(-1);
                Point2D west = pos.ShiftX(-1);

                return map.ContainsKey(north) && map[north] is ScaffoldTile
                    && map.ContainsKey(east) && map[east] is ScaffoldTile
                    && map.ContainsKey(south) && map[south] is ScaffoldTile
                    && map.ContainsKey(west) && map[west] is ScaffoldTile
                ;
            }).Select(tile => tile.Key);

            int sumOfAligntmentParameters = intersections.Select(p => p.GetX() * p.GetY()).Sum();
            Console.WriteLine("The sum of all the alignment parameters is: {0}", sumOfAligntmentParameters);
        }
    }
}
