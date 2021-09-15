using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class TheNBodyProblem : AdventOfCodePuzzle {
        public TheNBodyProblem() : base(2019, 12) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Extract celestial bodies (moons)
            CelestialBody[] celestialBodies = ParseInput(puzzleInput);

            // Create universe & simulate 1,000 steps
            Universe universe = new Universe(celestialBodies);
            universe.Simulate(1000);

            int totalSystemEnergy = universe.GetTotalEnergy();
            Console.WriteLine(totalSystemEnergy);

            // Part Two
            // Every dimension, x, y, and z is independant of one another.
            // I.e. they can be simulated seperately of one another.
            //
            // Let's make the assumption that every dimension movement is periodic.
            // If that is the case, and the periodicity is known for each dimension is known.
            // Then the periodicity of the whole system is the least common multiple of the
            // respective dimensions in the system.
            //
            // This could greatly reduce the simulations needed to be done.
            // Because the system inhibits the Markov's property, only one state that reoccurs need to be found.
            // After that state, it will be periodic.
            //
            // Meaning each dimension only need to be simulated (m+n+1)
            // where n is the periodicity and m is the number of steps until a periodicity cycle is entered.
            //
            // Let's try it out!
            //

            // Analyze each of the 3 dimensions (0-2) seperately
            int dimensionCount = 3;
            int bodyCount = celestialBodies.Length;
            int[] periodicities = new int[dimensionCount];

            for (int dimension = 0; dimension < dimensionCount; dimension++) {
                // Recreate the celestial bodies in one dimension
                CelestialBody[] oneDimensionalCelestialBodies = new CelestialBody[bodyCount];
                for (int i = 0; i < bodyCount; i++) {
                    int position = celestialBodies[i].GetDimension(dimension).GetPosition();

                    CelestialBody celestialBody = new CelestialBody(celestialBodies[i].GetName());
                    celestialBody.AddDimension(dimension, position);

                    oneDimensionalCelestialBodies[i] = celestialBody;
                }

                // Create universe
                Universe oneDimensionalUniverse = new Universe(oneDimensionalCelestialBodies);

                // Create hashset to store paste universes (states)
                HashSet<ImmutableUniverse> pastUniverses = new HashSet<ImmutableUniverse>();

                // Look for periodicity by simulating
                ImmutableUniverse matchingUniverse;
                ImmutableUniverse immutableUniverse;
                while (true) {
                    // If we can find the universe (state) in the previous uniervses (states), break
                    immutableUniverse = oneDimensionalUniverse.GetImmutable();
                    if (pastUniverses.TryGetValue(immutableUniverse, out matchingUniverse)) break;

                    // Otherwise
                    // Add universe (state) to old universes (states)
                    pastUniverses.Add(immutableUniverse);

                    // Simulate next step in time
                    oneDimensionalUniverse.Simulate(1);
                }

                //Console.WriteLine("Yay we have a match!");
                //Console.WriteLine(matchingUniverse);
                //Console.WriteLine(immutableUniverse);

                periodicities[dimension] = immutableUniverse.GetTimestep() - matchingUniverse.GetTimestep();
            }

            // Find the least common multiple
            long leastCommonMultiple = periodicities[0];
            for (int i = 1; i < dimensionCount; i++) {
                long periodicity = periodicities[i];

                leastCommonMultiple = MathExtensions.LCM(leastCommonMultiple, periodicity);
            }
            Console.WriteLine(leastCommonMultiple);
        }

        public CelestialBody[] ParseInput(string input) {
            string[] strObjects = input.SplitToLines();
            int N = strObjects.Length;

            string[] celestialNames = {
                "Io",
                "Europa",
                "Ganymede",
                "Callisto",
            };

            CelestialBody[] celestialBodies = new CelestialBody[N];
            for (int i = 0; i < N; i++) {
                string strObject = strObjects[i];
                int x = 0;
                int y = 0;
                int z = 0;

                foreach (string str in Regex.Replace(strObject, "[<>]", "").Split(",")) {
                    string s = str.Trim();

                    string[] parts = s.Split("=");
                    if (parts.Length != 2) throw new Exception("Invalid definition statement!");

                    // Extract
                    string var = parts[0];
                    int value = Int32.Parse(parts[1]);

                         if (var == "x") x = value;
                    else if (var == "y") y = value;
                    else if (var == "z") z = value;
                    else throw new Exception("Invalid variable!");
                }

                CelestialBody celestialBody = new CelestialBody(celestialNames[i]);
                celestialBody.AddDimension(0, x);
                celestialBody.AddDimension(1, y);
                celestialBody.AddDimension(2, z);

                celestialBodies[i] = celestialBody;
            }

            return celestialBodies;
        }
    }
}
