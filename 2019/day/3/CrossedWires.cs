using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class CrossedWires : AdventOfCodePuzzle {
        public CrossedWires() : base(2019, 3) {}

        override public void Solve() {
            string[] wireStrings = this.GetPuzzleInput().Split("\n");

            // Origin
            Point2D origin = Point2D.ORIGIN;

            // Extract instruction set
            List<string[]> wirePaths = new List<string[]>();
            foreach (string wireString in wireStrings) wirePaths.Add(wireString.Split(","));

            // Extract wire points
            List<List<Point2D>> wires = new List<List<Point2D>>();
            foreach (string[] wirePath in wirePaths) {
                List<Point2D> wirePoints = new List<Point2D>();

                // Add origin
                Point2D previousPoint = origin;
                wirePoints.Add(previousPoint);

                foreach (string step in wirePath) {
                    char direction = step[0];

                    Func<Point2D, int, Point2D> op = null;
                         if (direction == 'U') op = (p, dy) => new Point2D(p.GetX(), p.GetY()+dy);
                    else if (direction == 'D') op = (p, dy) => new Point2D(p.GetX(), p.GetY()-dy);
                    else if (direction == 'L') op = (p, dx) => new Point2D(p.GetX()-dx, p.GetY());
                    else if (direction == 'R') op = (p, dx) => new Point2D(p.GetX()+dx, p.GetY());
                    else System.Environment.Exit(2000); // This should not happen

                    int distance = Int32.Parse(step.Substring(1));

                    // Intepolate between previous point and next
                    for (int i = 0; i < distance; i++) {
                        Point2D p = op(previousPoint, 1);
                        wirePoints.Add(p);
                        previousPoint = p;
                    }
                }

                wires.Add(wirePoints);
            }

            HashSet<Point2D> intersections = new HashSet<Point2D>(wires[0]);
            for (int i = 1; i < wires.Count; i++) {
                intersections.IntersectWith(wires[i]);
            }

            List<int> manhanttanDistances = intersections.Select(p => p.GetManhattanSize()).ToList();
            manhanttanDistances.Sort();

            Console.WriteLine(manhanttanDistances[1]);
            Console.WriteLine("Done");

            // Part Two
            List<int> signalDelay = intersections.Select(p => wires.Select(wire => wire.IndexOf(p)).Sum()).ToList();
            signalDelay.Sort();

            Console.WriteLine(signalDelay[1]);
        }
    }
}
