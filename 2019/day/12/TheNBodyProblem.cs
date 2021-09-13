using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class TheNBodyProblem : AdventOfCodePuzzle {
        public TheNBodyProblem() : base(2019, 12) {}

        override public void Solve() {
            // Parse input
            string puzzleInput = this.GetPuzzleInput();

            List<CelestialBody> celestialBodies = new List<CelestialBody>();
            foreach (string strObject in puzzleInput.SplitToLines()) {
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

                celestialBodies.Add(new CelestialBody(x, y, z));
            }

            // Simulate
            int N;
            N = 1000;
            for (int i = 0; i < N; i++) {
                //Console.WriteLine("Step " + i);
                //foreach (var celestialBody in celestialBodies) Console.WriteLine(celestialBody);

                // Apply gravity
                foreach (var celestialBody in celestialBodies) celestialBody.ApplyGravity(celestialBodies);

                // Apply velocity
                foreach (var celestialBody in celestialBodies) celestialBody.ApplyVelocity();
            }

            int totalSystemEnergy = celestialBodies.Select(celestialBody => celestialBody.GetTotalEnergy()).Sum();
            Console.WriteLine(totalSystemEnergy);

        }
    }
}
