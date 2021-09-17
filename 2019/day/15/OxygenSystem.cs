using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class OxygenSystem : AdventOfCodePuzzle {
        public OxygenSystem() : base(2019, 15) {}

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

            // Init movement commands
            var moveCmds = new List<Func<Point2D, Point2D>>();
            moveCmds.Add(p => p.shiftY(1));
            moveCmds.Add(p => p.shiftX(1));
            moveCmds.Add(p => p.shiftY(-1));
            moveCmds.Add(p => p.shiftX(-1));

            // Part two
            // Beat the game?
            var map = new Dictionary<Point2D, MapTile>();
            var defaultTile = new UnknownTile();

            // Load the program
            computer.LoadProgram(program);

            // Add robot to map
            Point2D currentPosition = Point2D.ORIGIN;
            map[currentPosition] = new RobotTile(true);
            int prevCmd = 0;

            while (true) {
                // Draw map
                Console.WriteLine(MapToString(map, defaultTile, true));
                Console.Write("\n\n\n");

                int moveCmd;
                //moveCmd = GetUserInput();
                moveCmd = GetAIInput(map, currentPosition, moveCmds, prevCmd);
                Console.WriteLine("moveCmd: {0}", 1+moveCmd);

                // Run program
                computer.Run(ConvertToIntcodeInstruction(moveCmd));
                Point2D nextPosition = moveCmds[moveCmd](currentPosition);

                int statusCode = (int) computer.GetOutput();
                if (statusCode == 0) { // Robot hit a wall
                    map[nextPosition] = new WallTile();
                }
                else if (statusCode >= 1 && statusCode <= 2) { // Successful move
                    OccupiableTile currentTile = (OccupiableTile) map[currentPosition];
                    OccupiableTile nextTile = null;

                    if (map.ContainsKey(nextPosition)) nextTile = (OccupiableTile) map[nextPosition];
                    else {
                        if (statusCode == 1) nextTile = new RobotTile();
                        else if (statusCode == 2) {
                            nextTile = new OxygenSystemTile();
                        }
                        map[nextPosition] = nextTile;
                    }

                    currentTile.Vacate();
                    nextTile.Occupy();

                    currentPosition = nextPosition;
                    prevCmd = moveCmd;
                }
                else throw new Exception("Program corrupt!");
            }
            
            Console.WriteLine(MapToString(map, defaultTile, true));
            Console.Write("\n\n\n");
        }

        string MapToString(IEnumerable<KeyValuePair<Point2D, MapTile>> pixels, MapTile defaultTile) {
            return MapToString(pixels, defaultTile, false);
        }
        string MapToString(IEnumerable<KeyValuePair<Point2D, MapTile>> pixels, MapTile defaultTile, bool flipY) {
            char defaultSymbol = defaultTile.GetSymbol();
            int ySign = flipY ? -1 : 1;
            
            var xs = pixels.Select(pixel => pixel.Key.GetX());
            var ys = pixels.Select(pixel => pixel.Key.GetY());

            int xOffset = xs.Min();
            int yOffset = ys.Min() * ySign;
            int width = 1 + xs.Max() - xOffset;

            var sortedPixels = pixels.OrderBy(pixel => pixel.Key.GetY() * ySign).ThenBy(pixel => pixel.Key.GetX()).ToArray();

            StringBuilder sb = new StringBuilder();
            Point2D prevPosition = new Point2D(xOffset-1, yOffset);
            int prevX = 1 + prevPosition.GetX() - xOffset;

            for (int i = 0; i < sortedPixels.Length; i++) {
                Point2D thisPosition = sortedPixels[i].Key;

                //
                int dY = prevPosition.GetDeltaY(thisPosition) * ySign;
                int thisX = thisPosition.GetX() - xOffset;

                if (dY > 0) {
                    // Pad the rest of 'prev' row
                    for (int x = prevX; x < width; x++) sb.Append(defaultSymbol);
                    sb.Append('\n');

                    // Pad rows inbetween
                    for (int y = 1; y < dY; y++) {
                        for (int x = 0; x < width; x++) sb.Append(defaultSymbol);
                        sb.Append('\n');
                    }

                    // Pad the start of 'this' row
                    for (int x = 0; x < thisX; x++) sb.Append(defaultSymbol);
                }
                else for (int x = prevX; x < thisX; x++) sb.Append(defaultSymbol);

                // Print this tile
                MapTile thisTile = sortedPixels[i].Value;
                sb.Append(thisTile.GetSymbol());

                prevPosition = thisPosition;
                prevX = 1 + thisX;
            }

            // Pad end of the last row
            for (int x = prevX; x < width; x++) sb.Append(defaultSymbol);

            return sb.ToString();
        }

        int GetUserInput() {
            while (true) {
                var key = Console.ReadKey(false).Key;

                switch(key) {
                    case ConsoleKey.UpArrow: return 0;
                    case ConsoleKey.RightArrow: return 1;
                    case ConsoleKey.DownArrow: return 2;
                    case ConsoleKey.LeftArrow: return 3;
                    case ConsoleKey.Escape:
                        System.Environment.Exit(1);
                        return 0; // Just return something
                }
            }
        }
        
        int ConvertToIntcodeInstruction(int moveCmd) {
            int[] array = { 1, 4, 2, 3 };
            return array[moveCmd];
        }

        int GetAIInput(
            Dictionary<Point2D, MapTile> map,
            Point2D thisPos,
            List<Func<Point2D, Point2D>> moveCmds,
            int prevCmd
        ) {
            int[] moves = {
                prevCmd+1,
                prevCmd,
                prevCmd-1,
                prevCmd+2,
            };

            for (int i = 0; i < moves.Length; i++) {
                var move = MathExtensions.PositiveModulo(moves[i], moves.Length);

                var moveCmd = moveCmds[move];
                var nextPos = moveCmd(thisPos);
                if (IsOccupiable(map, nextPos)) return move;
            }

            return MathExtensions.PositiveModulo(moves[moves.Length-1], moves.Length);
        }

        bool IsOccupiable(Dictionary<Point2D, MapTile> map, Point2D pos) {
            if (!map.ContainsKey(pos)) return true;

            MapTile tile = map[pos];
            if (tile  is OccupiableTile) return true;

            return false;
        }

    }
}