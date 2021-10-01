using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class UniversalOrbitMap : AdventOfCodePuzzle {
        public UniversalOrbitMap() : base(2019, 6) {}

        override protected void SolvePuzzle(string puzzleInput) {
            string[] orbits = puzzleInput.SplitToLines();

            // Extract COM name
            Dictionary<SatelliteKey, Satellite> satellites = new Dictionary<SatelliteKey, Satellite>();

            foreach (var orbit in orbits) {
                string[] satelliteNames = orbit.Split(")");
                if (satelliteNames.Length != 2) throw new Exception("Corrupted input data");

                Satellite[] sats = satelliteNames.Select(satelliteName => {
                    SatelliteKey key = new SatelliteKey(satelliteName);
                    Satellite sat;

                    if (!satellites.TryGetValue(key, out sat)) {
                        sat = new Satellite(key);
                        satellites.Add(key, sat);
                    }

                    return sat;
                }).ToArray();

                Satellite host = sats[0];
                Satellite sat = sats[1];

                sat.SetHost(host);
            }

            int totalOrbits = satellites.Values.Select(satellite => satellite.GetTotalOrbits()).Sum();
            Console.WriteLine("The total number of direct and indirect orbits are: {0}", totalOrbits);

            // Part two
            List<Satellite> youHierarchy = GetSataliteHierarchy(satellites[new SatelliteKey("YOU")]);
            List<Satellite> sanHierarchy = GetSataliteHierarchy(satellites[new SatelliteKey("SAN")]);

            int i = 0;
            while (true) {
                // Find out how many are same orbits (i counter)
                Satellite a = youHierarchy[i];
                Satellite b = sanHierarchy[i];

                if (!a.Equals(b)) break;
                i++;
            }

            int minimumNumberOfTransfers = (youHierarchy.Count-1) + (sanHierarchy.Count-1) - 2*i;
            Console.WriteLine("The minimal number of orbit transfer to reach santa is: {0}", minimumNumberOfTransfers);
        }

        static List<Satellite> GetSataliteHierarchy(Satellite satellite) {
            List<Satellite> hierarchy = new List<Satellite>();
            hierarchy.Add(satellite);

            while (satellite.HasHost()) {
                satellite = satellite.GetHost();
                hierarchy.Add(satellite);
            }

            hierarchy.Reverse();
            return hierarchy;
        }
    }
}
