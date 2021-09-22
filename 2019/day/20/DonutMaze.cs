using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class DonutMaze : AdventOfCodePuzzle {
        public DonutMaze() : base(2019, 20) {}

        override protected string ProcessPuzzleInput(string puzzleInput) {
            return puzzleInput;
        }

        override protected void SolvePuzzle(string puzzleInput) {
            var startTile = ParseMaze(puzzleInput);

            var costMap = new Dictionary<MazeTile, int>();
            var explorer = new Queue<MazeTile>();

            // Add inital tile
            costMap[startTile] = 0;
            explorer.Enqueue(startTile);

            while (explorer.Count > 0) {
                var explore = explorer.Dequeue();
                var nextCost = costMap[explore]+1;

                foreach (var nextTile in explore.GetSteps()) {
                    var prevCost = costMap.GetValueOrDefault(nextTile, Int32.MaxValue);

                    if (nextCost < prevCost) {
                        costMap[nextTile] = nextCost;
                        explorer.Enqueue(nextTile);
                    }
                }
            }

            int steps = costMap.Where(e => e.Key.IsGoal()).Select(e => e.Value).Single();
            Console.WriteLine("The number of steps required to reach the end of the maze are: {0}", steps);
        }

        MazeTile ParseMaze(string puzzleInput) {
            //
            var portals = new Dictionary<Point2D, char>();
            var maze = new Dictionary<Point2D, MazeTile>();

            // Parse...
            string[] puzzleRows = puzzleInput.SplitToLines();
            for (int y = 0; y < puzzleRows.Length; y++) {
                string puzzleRow = puzzleRows[y];

                for (int x = 0; x < puzzleRow.Length; x++) {
                    char c = puzzleRow[x];

                    if (c == '.') {
                        var mazeTile = new MazeTile(x, -y);
                        maze[mazeTile] = mazeTile;
                    }
                    else if (c >= 'A' && c <= 'Z') {
                        portals[new Point2D(x, -y)] = c;
                    }
                }
            }

            // Add steps to maze tiles
            foreach (var mazeTile in maze.Values) {
                for (int i = 0; i < 4; i++) {
                    var p = mazeTile.Shift(i, 1);

                    if (maze.ContainsKey(p)) mazeTile.AddStep(i, maze[p]);
                }
            }

            var portalPairs = new Dictionary<string, List<(MazeTile MazeTile, int Direction)>>();

            // Resolve portals
            while (portals.Count > 0) {
                var portalEntryA = portals.First();
                Point2D portalPosA = portalEntryA.Key;
                char portalCharA = portalEntryA.Value;

                for (int i = 0; i < 4; i++) {
                    var portalPosB = portalPosA.Shift(i, 1);

                    // If the position exists
                    // That is the other part of the portal
                    // Multiple portals are never adjacent to one another
                    if (portals.ContainsKey(portalPosB)) {
                        var portalCharB = portals[portalPosB];

                        // Figure out direction
                        for (int j = 0; j < 2; j++) {
                            int dir = i+2*j;

                            var p = portalPosA.Shift(dir, 2-j);

                            if (maze.ContainsKey(p)) {
                                var mazeTile = maze[p];

                                // Special case for AA and ZZ
                                if (portalCharA == 'A' && portalCharB == 'A') {
                                    mazeTile.SetStart(true);
                                }
                                else if (portalCharA == 'Z' && portalCharB == 'Z') {
                                    mazeTile.SetGoal(true);
                                }
                                else {
                                    var key = String.Format((portalCharA > portalCharB) ? "{0}{1}" : "{1}{0}", portalCharA, portalCharB);
                                    var portalPairList = portalPairs.GetValueOrDefault(key, new List<(MazeTile, int)>());

                                    portalPairList.Add((mazeTile, dir-2));
                                }

                                // Finally remove A B (they are handled)
                                portals.Remove(portalPosA);
                                portals.Remove(portalPosB);
                                
                                // Now once the portal(s)
                                goto continueOuterLoop;
                            }
                        }
                    }
                }

                // If we got here, it no good.
                throw new Exception("Could not find other portal components, corrupt map!");

                continueOuterLoop:;
            }

            // For every pair
            foreach (var portalPairList in portalPairs.Values) {
                if (portalPairList.Count != 2)
                    throw new Exception("Single portal!? corrupt map!");

                var portalA = portalPairList[0];
                var portalB = portalPairList[1];

                maze[portalA.MazeTile].AddStep(portalA.Direction, portalB.MazeTile);
                maze[portalB.MazeTile].AddStep(portalB.Direction, portalA.MazeTile);
            }

            // All done, return start
            return maze.Values.Where(mazeTile => mazeTile.IsStart()).Single();
        }
    }

    class MazeTile : Point2D {

        HashSet<MazeTile> Steps = new HashSet<MazeTile>();
        bool Goal = false;
        bool Start = false;

        public MazeTile(int x, int y) : base(x, y) {}

        public void AddStep(int dir, MazeTile step) {
            Steps.Add(step);
        }

        public IEnumerable<MazeTile> GetSteps() {
            return Steps;
        }

        public void SetGoal(bool goal) {
            Goal = goal;
        }

        public bool IsGoal() {
            return Goal;
        }
        
        public void SetStart(bool start) {
            Start = start;
        }

        public bool IsStart() {
            return Start;
        }
    }
}
