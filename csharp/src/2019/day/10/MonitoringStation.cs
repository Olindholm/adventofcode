using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class MonitoringStation : AdventOfCodePuzzle {
        public MonitoringStation() : base(2019, 10) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load puzzle input
            string[] astroidMap = puzzleInput.SplitToLines();

            List<Astroid> astroids = new List<Astroid>();
            for (int y = 0; y < astroidMap.Length; y++) {
                for (int x = 0; x < astroidMap[y].Length; x++) {
                    char c = astroidMap[y][x];

                         if (c == '.') {} // Empty space, do nothing
                    else if (c == '#') astroids.Add(new Astroid(x, y));
                    else throw new Exception("Unsupported map entitiy");
                }
            }

            var monitoringStation = astroids.Aggregate<Astroid, (Astroid Astroid, int AstroidInSight)>((null, -1), (candidatingAstroid, nextAstroid) => {
                int astroidInSight = astroids.Where(astroid => astroid != nextAstroid).Select(astroid => nextAstroid.GetAngle(astroid)).ToHashSet().Count;
                return (astroidInSight > candidatingAstroid.AstroidInSight) ? (nextAstroid, astroidInSight) : candidatingAstroid;
            });

            Console.WriteLine("The best monitoring station is located at: ({0}, {1})", monitoringStation.Astroid.GetX(), monitoringStation.Astroid.GetY());
            Console.WriteLine("The number of other astroids that can be detected from there are: {0}", monitoringStation.AstroidInSight);

            // Part Two
            Astroid laser = monitoringStation.Astroid;
            IEnumerable<IGrouping<double, Astroid>> astroidGroupByAngle = astroids
            .Where(astroid => astroid != monitoringStation.Astroid)
            .GroupBy(
                astroid => laser.GetAngle(astroid),
                astroid => astroid
            );

            List<(double Angle, List<Astroid> Astroids)> astroidsByAngle = new List<(double Angle, List<Astroid> Astroids)>();
            foreach (IGrouping<double, Astroid> astroidGroup in astroidGroupByAngle) {
                // Extract angle and astroids
                double angle = astroidGroup.Key;
                List<Astroid> astroidAngleList = new List<Astroid>(astroidGroup);

                // Sort astroids by distance from laser
                astroidAngleList.Sort((a, b) => {
                    int aDistance = laser.GetManhattanDistance(a);
                    int bDistance = laser.GetManhattanDistance(b);
                    return aDistance - bDistance;
                });

                astroidsByAngle.Add((angle, astroidAngleList));
            }

            // Sort astroids by angle
            astroidsByAngle.Sort((a, b) => (a.Angle > b.Angle) ? 1 : -1);

            // Extract astroids in order for lasering
            List<Astroid> astroidsToLaser = new List<Astroid>();
            int astroidCount = astroids.Count;

            int i = 0;
            while (true) {
                List<Astroid> astroidAngleList = astroidsByAngle[i].Astroids;

                // Add astroid to laser list
                Astroid astroid = astroidAngleList[0];
                astroidsToLaser.Add(astroid);

                // Remove astroid from astroids in this angle
                astroidAngleList.RemoveAt(0);

                // If no more astroids in this angle, remove from list
                if (astroidAngleList.Count == 0) {
                    astroidsByAngle.RemoveAt(i);
                    i--;
                }

                // If no more astroids, break else increment i
                if (astroidsByAngle.Count == 0) break;
                i = (i+1) % astroidsByAngle.Count;
            }

            Astroid astroid200 = astroidsToLaser[200-1];
            int answer = 100*astroid200.GetX() + astroid200.GetY();

            Console.WriteLine("The 200th astroid to be vaporized is located at: ({0}, {1})", astroid200.GetX(), astroid200.GetY());
            Console.WriteLine("The answer is: {0}", answer);

            // Detta sorteas inte på rätt sätt...
            // den ger alla stenar av samma vinkel först, inge bra
            // använd group by istället. Och sne hämta för var vinkel en sten
            // in i el lista.. då kan de bli rätt ordnin
        }
    }
}
