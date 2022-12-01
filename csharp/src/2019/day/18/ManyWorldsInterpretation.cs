using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class ManyWorldsInterpretation : AdventOfCodePuzzle {
        public ManyWorldsInterpretation() : base(2019, 18) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Parse map
            string[] puzzleLines = puzzleInput.SplitToLines();
            int width = puzzleLines[0].Length;
            int height = puzzleLines.Length;

            var map = new Dictionary<Point2D, char>();
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) map[new Point2D(x - width/2, -(y - height/2))] = puzzleLines[y][x];

            // Part one
            // Find least steps to collect all keys
            int fewestSteps;
            fewestSteps = FindAllKeysInFewestSteps(map);
            PrintResult(map, fewestSteps);

            // Part two
            Console.Write("\n\n\n");
            // Modify map spawn
            Point2D initialPos = map.Where(e => e.Value == '@').Select(e => e.Key).Single();
            for (int x = 0; x < 3; x++) for (int y = 0; y < 3; y++) {
                char c = (y % 2 == 0 && x % 2 == 0) ? '@' : '#';
                var p = new Point2D(initialPos.GetX()-1+x, initialPos.GetY()-1+y);
                map[p] = c;
            }

            // Find least steps to collect all keys (again)
            fewestSteps = FindAllKeysInFewestSteps(map);
            PrintResult(map, fewestSteps);
        }

        private void PrintResult(Dictionary<Point2D, char> map, int fewestSteps) {
            Console.WriteLine("The underground vault looks as:");
            Console.WriteLine(MapToString(map));
            Console.WriteLine("The least amount of steps needed to collect all keys are: {0}", fewestSteps);
        }

        int FindAllKeysInFewestSteps(Dictionary<Point2D, char> map) {
            // Find initial(s)
            var intialPositions = map.Where(e => e.Value == '@').Select(e => e.Key);

            // Find keys and their positions
            var allKeyPositions = map.Where(e => IsKey(e.Value)).Select(e => e.Key);
            var allKeys = allKeyPositions.Select(p => map[p]);

            // Determine the all point a to point b
            // (Where a and b are interesting points)
            // (I.e. keys, or initial position)
            var pathCosts = new Dictionary<Point2D, Dictionary<Point2D, ManyWorldsCost>>();

            var positions = intialPositions.Concat(allKeyPositions);
            foreach (var pos in positions) {
                var pathCost = new Dictionary<Point2D, ManyWorldsCost>();
                foreach (var keyTuple in FindAllKeysAndCosts(map, pos)) pathCost[keyTuple.Position] = keyTuple.Cost;
                pathCosts[pos] = pathCost;
            }

            var stateCosts = new Dictionary<ManyWorldsState, int>();
            return FindRemainingKeysInFewestSteps(map, pathCosts, stateCosts, new ManyWorldsState(intialPositions), allKeys);
        }

        int FindRemainingKeysInFewestSteps(
            Dictionary<Point2D, char> map,
            Dictionary<Point2D, Dictionary<Point2D, ManyWorldsCost>> pathCosts,
            Dictionary<ManyWorldsState, int> stateCosts,
            ManyWorldsState thisState,
            IEnumerable<char> remainingKeys
        ) {
            //Console.Write("\n" + thisState);

            var KeysToFind = new HashSet<char>(remainingKeys);
            if (KeysToFind.Count == 0) return 0; // If no more keys to find, don't bother

            // Check if this state already has a cost
            // Then it has already been calculcated
            // and need not be recalculated.
            // Just return cached value
            if (stateCosts.ContainsKey(thisState)) {
                //Console.Write(" (Already Cached, skip!)");
                return stateCosts[thisState];
            }

            // Otherwise...
            // Find the cheapest of all substates
            var thisKeys = thisState.GetKeys();
            var thisRobotPositions = thisState.GetRobotPositions();

            int leastCost = Int32.MaxValue;

            for (int robot = 0; robot < thisRobotPositions.Count; robot++) {
                var robotPos = thisRobotPositions[robot];

                var nextKeyPositions = pathCosts[robotPos]
                .Where(e => e.Value.HasRequiredKeys(thisKeys))
                .Select(e => e.Key)
                .Where(p => KeysToFind.Contains(map[p]))
                ;

                foreach (var nextKeyPos in nextKeyPositions) {
                    // Get key and cost
                    var nextKey = map[nextKeyPos];
                    var nextCost = pathCosts[robotPos][nextKeyPos].GetCost();

                    // Create new (next) state
                    var nextRobotPositions = thisRobotPositions.ReplaceIndex(robot, nextKeyPos);
                    var nextKeys = thisKeys.Append(nextKey);

                    var nextState = new ManyWorldsState(nextRobotPositions, nextKeys);
                    var nextStateCost = FindRemainingKeysInFewestSteps(map, pathCosts, stateCosts, nextState, KeysToFind.Exclude(nextKey));

                    // Update least cost
                    leastCost = Math.Min(leastCost, nextCost + nextStateCost);
                }
            }

            // Insert this state's cost into the state cost cache
            stateCosts.Add(thisState, leastCost);

            return leastCost;
        }


        IEnumerable<(Point2D Position, char Key, ManyWorldsCost Cost)> FindAllKeysAndCosts(Dictionary<Point2D, char> map, Point2D pos) {
            var keyPositions = new HashSet<Point2D>();

            // Explorer and costs
            var explorer = new UniqueQueue<Point2D>();
            var costs = new Dictionary<Point2D, ManyWorldsCost>();

            // Add initial
            EnqueueIfCheaper(explorer, costs, pos, new ManyWorldsCost(0));

            while (explorer.Count > 0) {
                var thisPos = explorer.Dequeue();
                var thisCost = costs[thisPos];

                for (int dir = 0; dir < 4; dir++) {
                    var nextPos = thisPos.Shift(dir, 1);

                    if (IsOccupiable(map, nextPos)) {
                        char c = map[nextPos];

                        // If key, add to list
                        if (IsKey(c)) keyPositions.Add(nextPos);

                        // If door, add to keys (required)
                        var nextRequiredKeys = thisCost.GetRequiredKeys();
                        if (IsDoor(c)) nextRequiredKeys = nextRequiredKeys.Append(DoorToKey(c));

                        // Create the cost of this (next) position
                        var nextCost = new ManyWorldsCost(thisCost.GetCost()+1, nextRequiredKeys);

                        EnqueueIfCheaper(explorer, costs, nextPos, nextCost);
                    }
                }
            }

            return keyPositions.Select(p => (p, map[p], costs[p]));
        }

        bool EnqueueIfCheaper<T, C>(UniqueQueue<T> queue, Dictionary<T, C> costs, T next, C nextCost) where C : IComparable<C> {

            if (costs.ContainsKey(next) && nextCost.CompareTo(costs[next]) >= 0) return false;

            costs[next] = nextCost;
            queue.Enqueue(next);
            return true;
        }

        bool IsOccupiable(Dictionary<Point2D, char> map, Point2D pos) {
            if (!map.ContainsKey(pos)) return false;
            char c = map[pos];

            if (c == '#') return false;
            if (c == '@') return true;
            if (c == '.') return true;
            if (IsKey(c)) return true;
            if (IsDoor(c)) return true;

            // If we got down here, the character is unknown?
            // Throw exception
            throw new Exception(String.Format("Bad map character: {0}", map[pos]));

            // Just return something
            // This code will never be reached.
            // Disable warning for Unreachable code (CS0162)
            #pragma warning disable CS0162
            return false;
            #pragma warning restore CS0162
        }

        bool IsKey(char c) {
            return (c >= 'a' && c <= 'z');
        }

        bool IsDoor(char c) {
            return (c >= 'A' && c <= 'Z');
        }

        char DoorToKey(char c) {
            if (!IsDoor(c)) throw new Exception("No key exists for that, it is not a door!");
            return Char.ToLower(c);
        }

        public static string MapToString<E>(IEnumerable<KeyValuePair<Point2D, E>> pixels, bool flipY = false) {
            return MapToString(pixels, "░", flipY);
        }


        public static string MapToString<E>(IEnumerable<KeyValuePair<Point2D, E>> pixels, E defaultValue, bool flipY = false) {
            return MapToString(pixels, defaultValue.ToString(), flipY);
        }

        public static string MapToString<E>(IEnumerable<KeyValuePair<Point2D, E>> pixels, string defaultValue, bool flipY = false) {
            if (pixels.Count() == 0) return "";

            int ySign = flipY ? -1 : 1;

            var xs = pixels.Select(pixel => pixel.Key.GetX());
            var ys = pixels.Select(pixel => pixel.Key.GetY());

            int xOffset = xs.Min();
            int yOffset = ys.Min();
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
                    for (int x = prevX; x < width; x++) sb.Append(defaultValue);
                    sb.Append('\n');

                    // Pad rows inbetween
                    for (int y = 1; y < dY; y++) {
                        for (int x = 0; x < width; x++) sb.Append(defaultValue);
                        sb.Append('\n');
                    }

                    // Pad the start of 'this' row
                    for (int x = 0; x < thisX; x++) sb.Append(defaultValue);
                }
                else for (int x = prevX; x < thisX; x++) sb.Append(defaultValue);

                // Print this tile
                E thisValue = sortedPixels[i].Value;
                sb.Append(thisValue.ToString());

                prevPosition = thisPosition;
                prevX = 1 + thisX;
            }

            // Pad end of the last row
            for (int x = prevX; x < width; x++) sb.Append(defaultValue);

            return sb.ToString();
        }
    }
}
