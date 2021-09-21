using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class ManyWorldsInterpretation : AdventOfCodePuzzle {
        public ManyWorldsInterpretation() : base(2019, 18) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Decode map
            string[] puzzleLines = puzzleInput.SplitToLines();
            int width = puzzleLines[0].Length;
            int height = puzzleLines.Length;

            var map = new Dictionary<Point2D, char>();
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) map[new Point2D(x, y)] = puzzleLines[y][x];

            // Part one
            var costMap = new Dictionary<KeyState, Dictionary<Point2D, int>>();
            var explorer = new Queue<(KeyState Keys, Point2D Position)>();

            // Initial(s)
            KeyState initialKeys = new KeyState();
            Point2D initialPos = map.Where(e => e.Value == '@').Select(e => e.Key).Single();

            // Add initial(s) to cost, and explorer
            costMap.GetValueOrDefault(initialKeys, new Dictionary<Point2D, int>())[initialPos] = 0;
            explorer.Enqueue((initialKeys, initialPos));

            while (explorer.Count > 0) {
                var explore = explorer.Dequeue();
                var keys = explore.Keys;
                var pos = explore.Position;
                var nextCost = costMap[keys][pos]+1;

                for (int dir = 0; dir < 4; dir++) {
                    var nextPos = pos.Shift(dir, 1);

                    if (IsOccupiable(map, nextPos, keys)) {
                        var c = map[nextPos];
                        var nextKeys = IsKey(c) ? keys.Add(c) : keys;

                        var prevCost = costMap
                        .GetValueOrDefault(nextKeys, new Dictionary<Point2D, int>())
                        .GetValueOrDefault(nextPos, Int32.MaxValue)
                        ;

                        if (nextCost < prevCost) {
                            costMap[nextKeys][nextPos] = nextCost;
                            explorer.Enqueue((nextKeys, nextPos));
                        }
                    }
                }
            }

            // Find (all keys state)
            KeyState allKeys = new KeyState(map.Values.Where(c => IsKey(c)));
            int leastStepsToCollectAllKeys = costMap[allKeys].Values.Min();

            Console.WriteLine("The underground vault looks as:");
            Console.WriteLine(puzzleInput);
            Console.WriteLine("The least amount of steps needed to collect all keys are: {0}", leastStepsToCollectAllKeys);
        }

        bool IsOccupiable(Dictionary<Point2D, char> map, Point2D pos, KeyState keys) {
            if (!map.ContainsKey(pos)) return false;
            if (map[pos] == '#') return false;
            if (map[pos] == '@') return true;
            if (map[pos] == '.') return true;
            if (IsKey(map[pos])) return true;

            // If door, see if possesses the key
            if (map[pos] >= 'A' && map[pos] <= 'Z') {
                return keys.PossessesKey(Char.ToLower(map[pos]));
            }

            // If we got down here, the character is unknown?
            throw new Exception(String.Format("Bad map character: {0}", map[pos]));
            return false;
        }
        
        bool IsKey(char c) {
            return (c >= 'a' && c <= 'z');
        }
    }
}
