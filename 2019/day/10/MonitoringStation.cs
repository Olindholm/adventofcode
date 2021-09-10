using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class MonitoringStation : AdventOfCodePuzzle {
        public MonitoringStation() : base(2019, 10) {}

        override public void Solve() {
            // Load puzzle input
            string[] astroidMap = this.GetPuzzleInput().SplitToLines();

            List<Point2D> astroids = new List<Point2D>();
            for (int y = 0; y < astroidMap.Length; y++) {
                for (int x = 0; x < astroidMap[y].Length; x++) {
                    char c = astroidMap[y][x];

                         if (c == '.') {} // Empty space, do nothing
                    else if (c == '#') astroids.Add(new Point2D(x, y));
                    else throw new Exception("Unsupported map entitiy");
                }
            }

            int maxVisibleAstroids = astroids.Select(centerAstroid => {
                return astroids.Select(otherAstroid => (centerAstroid != otherAstroid) ? centerAstroid.GetAngle(otherAstroid) : Double.NaN).ToHashSet().Count - 1;
            }).Max();

            Console.WriteLine(maxVisibleAstroids);
        }
    }
}
