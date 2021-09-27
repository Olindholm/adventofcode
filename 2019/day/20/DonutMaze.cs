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
            var maze = ParseMaze(puzzleInput);

            var initialState = new DonutMazeState(maze.Start);
            int steps;

            // Part one
            steps = EscapeMazeInFewestSteps(initialState, maze.Final, false, maze.Portals, maze.PathCosts);
            Console.WriteLine("The number of steps required to reach the end of the maze are: {0}", steps);

            // Part two
            steps = EscapeMazeInFewestSteps(initialState, maze.Final, true, maze.Portals, maze.PathCosts);
            Console.WriteLine("The number of steps required to reach the end of the maze are: {0}", steps);
        }

        int EscapeMazeInFewestSteps(
            DonutMazeState thisState,
            Point2D goal,
            bool recursiveSpaces,
            Dictionary<Point2D, DonutMazePortal> portals,
            Dictionary<Point2D, Dictionary<Point2D, int>> pathCosts
        ) {
            return EscapeMazeInFewestSteps(thisState, goal, recursiveSpaces, portals, pathCosts, new Dictionary<DonutMazeState, int>(), Enumerable.Empty<DonutMazeState>());
        }

        int EscapeMazeInFewestSteps(
            DonutMazeState thisState,
            Point2D goal,
            bool recursiveSpaces,
            Dictionary<Point2D, DonutMazePortal> portals,
            Dictionary<Point2D, Dictionary<Point2D, int>> pathCosts,
            Dictionary<DonutMazeState, int> stateCosts,
            IEnumerable<DonutMazeState> prevStates
        ) {
            // Debug state
            //var usedPortals = prevStates.Reverse().Skip(1).Select(state => portals[state.GetPosition()].GetName() + "(" + state.GetLevel() + ")");
            //Console.WriteLine(String.Join(" => ", usedPortals));

            var thisPos = thisState.GetPosition();

            // If we're on goal! we found it!
            if (thisPos.Equals(goal))
                return thisState.GetLevel() == 0 ? 0 : Int32.MaxValue;
            
            // Check if this state already has a cost
            // Then it has already been calculcated
            // and need not be recalculated.
            // Just return cached value
            if (stateCosts.ContainsKey(thisState)) {
                //Console.Write(" (Already Cached, skip!)");
                return stateCosts[thisState];
            }

            // Check for recursive loops
            int leastCost = Int32.MaxValue;
            if (prevStates.Any(prevState => {
                if (!prevState.EqualsIgnoreLevel(thisState)) return false;
                if (Math.Abs(thisState.GetLevel()) >= Math.Abs(prevState.GetLevel())) {
                    if (thisState.GetLevel() % 2 == prevState.GetLevel() % 2) return true;
                }

                return false;
            })) return leastCost;

            var pathCost = pathCosts[thisPos];
            var nextPositions = pathCost.Keys;

            foreach (var nextPos in nextPositions) {
                var nextCost = pathCost[nextPos];
                var nextState = new DonutMazeState(nextPos, thisState.GetLevel());

                // Check is portal is available, then use
                if (portals.ContainsKey(nextPos)) { // Actually... this is always true
                    //Console.WriteLine("Take Portal: {0}", portals[nextPos].GetName());
                    nextState = portals[nextPos].Use(nextState, recursiveSpaces);
                    nextCost += 1;
                }

                var nextStateCost = EscapeMazeInFewestSteps(nextState, goal, recursiveSpaces, portals, pathCosts, stateCosts, prevStates.Prepend(thisState));

                // Update least cost
                leastCost = Math.Min(leastCost, MathExtensions.SafeOverflowAdd(nextCost, nextStateCost));
            }

            // Insert this state's cost into the state cost cache
            stateCosts.Add(thisState, leastCost);

            return leastCost;
        }

        (
            Point2D Start,
            Point2D Final,
            Dictionary<Point2D, DonutMazePortal> Portals,
            Dictionary<Point2D, Dictionary<Point2D, int>> PathCosts
        ) ParseMaze(string puzzleInput) {
            // Parse maze characters
            var portalPositions = new Dictionary<Point2D, char>();
            var maze = new HashSet<Point2D>();

            string[] puzzleRows = puzzleInput.SplitToLines();

            int height = puzzleRows.Length;
            int width = puzzleInput.Length / height;
            Point2D center = new Point2D(width/2, -height/2);

            for (int y = 0; y < puzzleRows.Length; y++) {
                string puzzleRow = puzzleRows[y];

                for (int x = 0; x < puzzleRow.Length; x++) {
                    char c = puzzleRow[x];
                    var p = new Point2D(x, -y);

                    if (c == '.') maze.Add(p);
                    else if (c >= 'A' && c <= 'Z') portalPositions[new Point2D(x, -y)] = c;
                }
            }

            // Resolve portal pairs
            var portalPairs = new Dictionary<string, List<Point2D>>();

            while (portalPositions.Count > 0) {
                var portalEntryA = portalPositions.First();
                Point2D portalPosA = portalEntryA.Key;
                char portalCharA = portalEntryA.Value;

                for (int i = 0; i < 4; i++) {
                    var portalPosB = portalPosA.Shift(i, 1);

                    // If the position exists
                    // That is the other part of the portal
                    // Multiple portals are never adjacent to one another
                    if (portalPositions.ContainsKey(portalPosB)) {
                        var portalCharB = portalPositions[portalPosB];

                        // Find portal entry position
                        // (The closest traversable positiion)
                        for (int j = 0; j < 2; j++) {
                            int dir = i+2*j;

                            var p = portalPosA.Shift(dir, 2-j);

                            if (maze.Contains(p)) {
                                var key = String.Format((portalCharA > portalCharB) ? "{0}{1}" : "{1}{0}", portalCharA, portalCharB);
                                var portalPairList = portalPairs.GetValueOrDefault(key, new List<Point2D>());

                                portalPairList.Add(p);

                                // Finally remove A B (they are handled)
                                portalPositions.Remove(portalPosA);
                                portalPositions.Remove(portalPosB);
                                
                                // Now once the portal(s)
                                goto continueOuterLoop;
                            }
                        }
                    }
                }

                // If we got here, it no good.
                throw new Exception("Corrupt map!");

                continueOuterLoop:;
            }

            var portals = new Dictionary<Point2D, DonutMazePortal>();
            Point2D startPos = null, finalPos = null;

            // Create (bind) portal pairs
            foreach (var portalPairEntry in portalPairs) {
                var portalName = portalPairEntry.Key;
                var portalPairList = portalPairEntry.Value;

                if (portalPairList.Count == 1) {
                    var portalPos = portalPairList[0];

                    // Start or final pos?
                         if (portalName.Equals("AA")) startPos = portalPos;
                    else if (portalName.Equals("ZZ")) finalPos = portalPos;
                    else throw new Exception("Single portal!? Corrupt map!");
                }
                else if (portalPairList.Count == 2) {
                    var portalAPos = portalPairList[0];
                    var portalBPos = portalPairList[1];

                    int portalAInc = IsInner(center, portalAPos) ? 1 : -1;
                    int portalBInc = IsInner(center, portalBPos) ? 1 : -1;

                    var portalA = new DonutMazePortal(portalName, portalAPos, portalBPos, portalAInc);
                    var portalB = new DonutMazePortal(portalName, portalBPos, portalAPos, portalBInc);

                    //Console.WriteLine(portalA);
                    //Console.WriteLine(portalB);

                    portals[portalAPos] = portalA;
                    portals[portalBPos] = portalB;
                }
                else throw new Exception("Corrupt map!");
            }

            // Ensure we found start and final pos
            if (startPos == null || finalPos == null) throw new Exception("Start/Final pos not found! Corrupt map!");

            // Create cost map (path costs)
            var pathCosts = new Dictionary<Point2D, Dictionary<Point2D, int>>();

            var originPositions = portals.Keys.Append(startPos);
            var destinationPositions = portals.Keys.Append(finalPos);

            foreach(var pos in originPositions) {
                var pathCost = new Dictionary<Point2D, int>();
                foreach (var pathTuple in FindCostsForPositions(maze, pos, destinationPositions.Exclude(pos))) pathCost[pathTuple.Position] = pathTuple.Cost;
                pathCosts[pos] = pathCost;
            }

            // All done, return start
            return (startPos, finalPos, portals, pathCosts);
        }

        IEnumerable<(Point2D Position, int Cost)> FindCostsForPositions(HashSet<Point2D> map, Point2D pos, IEnumerable<Point2D> positionsToFind) {
            // Explorer and costs
            var explorer = new UniqueQueue<Point2D>();
            var costs = new Dictionary<Point2D, int>();

            // Add initial
            EnqueueIfCheaper(explorer, costs, pos, 0);

            while (explorer.Count > 0) {
                var thisPos = explorer.Dequeue();
                var nextCost = costs[thisPos]+1;

                for (int dir = 0; dir < 4; dir++) {
                    var nextPos = thisPos.Shift(dir, 1);

                    if (map.Contains(nextPos)) EnqueueIfCheaper(explorer, costs, nextPos, nextCost);
                }
            }

            return positionsToFind.Where(p => costs.ContainsKey(p)).Select(p => (p, costs[p]));
        }

        bool IsInner(Point2D center, Point2D p) {
            var xFrac = 0.75;
            var yFrac = 0.75;

            var xInside = Math.Abs(p.GetDeltaX(center)) < xFrac * Math.Abs(center.GetX());
            var yInside = Math.Abs(p.GetDeltaY(center)) < yFrac * Math.Abs(center.GetY());

            return (xInside && yInside);
        }

        bool EnqueueIfCheaper<T, C>(UniqueQueue<T> queue, Dictionary<T, C> costs, T next, C nextCost) where C : IComparable<C> {
            if (costs.ContainsKey(next) && nextCost.CompareTo(costs[next]) >= 0) return false;

            costs[next] = nextCost;
            queue.Enqueue(next);
            return true;
        }
    }

}
