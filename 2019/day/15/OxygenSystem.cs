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
            var costMap = new Dictionary<Point2D, int>();
            var defaultTile = new UnknownTile();

            // Load the program
            computer.LoadProgram(program);

            // Add robot to map
            Point2D currentPosition = Point2D.ORIGIN;
            map[currentPosition] = new RobotTile(true);
            costMap[currentPosition] = 0;
            int prevCmd = 0;

            Point2D goalPosition = null;
            while (true) {
                // Draw map
                //Console.WriteLine(MapToString(map, defaultTile, true));
                //Console.Write("\n\n\n");

                int moveCmd;
                //moveCmd = GetUserInput();
                moveCmd = GetAIInput(map, currentPosition, moveCmds, prevCmd);

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
                            goalPosition = nextPosition;
                        }
                        map[nextPosition] = nextTile;
                    }

                    // Update map
                    currentTile.Vacate();
                    nextTile.Occupy();

                    // Update cost map
                    int thisCost = costMap[currentPosition]+1;
                    int nextCost = Int32.MaxValue;
                    if (costMap.ContainsKey(nextPosition)) nextCost = costMap[nextPosition];
                    if (thisCost < nextCost) costMap[nextPosition] = thisCost;

                    // Update position and move command
                    currentPosition = nextPosition;
                    prevCmd = moveCmd;
                }
                else throw new Exception("Program corrupt!");

                // Break when goal found and back at origin
                // (The whole map needs to be explored)
                if (goalPosition != null && currentPosition.Equals(Point2D.ORIGIN)) break;
            }

            Console.WriteLine("The whole area has been explored and looks like:");
            Console.WriteLine(MapToString(map, defaultTile, true));
            Console.WriteLine("The fewest number of steps to the oxygen system is: {0}", costMap[goalPosition]);

            // Part two
            var oxygenMap = new Dictionary<Point2D, int>();
            var explorerPositions = new Queue<Point2D>();

            // Add oxygen system to map
            oxygenMap[goalPosition] = 0;
            explorerPositions.Enqueue(goalPosition);

            while (explorerPositions.Count > 0) {
                var thisPos = explorerPositions.Dequeue();
                int thisMin = oxygenMap[thisPos]+1;

                // Explore adjacent positions
                // (if walkable)
                foreach (var moveCmd in moveCmds) {
                    var nextPos = moveCmd(thisPos);

                    // Check if occupiable
                    // Could use IsOccupiable(map, nextPos)
                    // But it's less safe, because in the
                    // unlikely scenario that origin has two
                    // roads to walk, parts of the map could
                    // be unexplored, causing oxygen to leak into
                    // unexplored area, and thus "space"
                    // This would use loop forever...
                    //
                    // Instead, since we assume the whole map is
                    // explored, all valid points should exist in
                    // the cost map. So check if the entry exists
                    // in that?
                    if (costMap.ContainsKey(nextPos)) {
                        int prevMin = Int32.MaxValue;
                        if (oxygenMap.ContainsKey(nextPos)) prevMin = oxygenMap[nextPos];
                        if (thisMin < prevMin) {
                            oxygenMap[nextPos] = thisMin;
                            explorerPositions.Enqueue(nextPos);
                        }
                    }
                }
            }

            int timeToFillEverything = oxygenMap.Values.Max();
            Console.WriteLine("The time it takes to fill the entire area with oxygen is: {0}", timeToFillEverything);
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
