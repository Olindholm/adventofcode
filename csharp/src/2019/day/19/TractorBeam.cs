using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class TractorBeam : AdventOfCodePuzzle {
        public TractorBeam() : base(2019, 19) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Init computer
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            computer.AddInstruction(new IntcodeInput());
            computer.AddInstruction(new IntcodeOutput());
            computer.AddInstruction(new IntcodeJumpIfTrue());
            computer.AddInstruction(new IntcodeJumpIfFalse());
            computer.AddInstruction(new IntcodeLessThan());
            computer.AddInstruction(new IntcodeEquals());
            computer.AddInstruction(new IntcodeAdjustRelativeBase());

            // Part one
            int initialSize = 50;
            int[,] affectorMatrix = ProbeBeamArea(program, initialSize);

            int numberOfAffectedPoints = 0;
            for (int i = 0; i < initialSize; i++) for (int j = 0; j < initialSize; j++) if (affectorMatrix[i, j] == 1) numberOfAffectedPoints++;

            Console.WriteLine("The tractor beam looks like:");
            Console.Write(SpaceImageFormat.ImageToString(affectorMatrix));
            Console.WriteLine("The number of affected points within the 50x50 area is: {0}", numberOfAffectedPoints);

            // Part two
            int size = 100;

            Func<int, bool> predicate = (c => c == 1);
            var a = FindMostOuterPoint(affectorMatrix, predicate);
            var b = FindMostOuterPoint(affectorMatrix, predicate, true);

            var theta =             Point2D.ORIGIN.GetAngle(a) + Math.PI/4;
            var gamma = Math.PI/2 - Point2D.ORIGIN.GetAngle(b) + Math.PI/4;
            var alpha = Math.PI - theta - gamma;

            var dist = Math.Sqrt(2) * size * Math.Sin(gamma) / Math.Sin(alpha);

            var phi = theta - Math.PI/4;

            var xDist = (int) (dist * Math.Cos(phi) - size);
            var yDist = (int) (dist * Math.Sin(phi));

            var margin = 50;
            var totalSize = size + 2*margin;
            int[,] beamMatrix = ProbeBeamArea(program, totalSize, xDist-margin, yDist-margin);

            int x = 0;
            int y = 0;
            while (true) {
                // If we have not found a fit?
                // And excceeded array indicies?
                // We're in big trouble!
                if (x >= totalSize - size && y >= totalSize - size) {
                    throw new Exception("This is not good! Array out of bounds and no square could fit!?");
                }

                // Loop through x-axis
                for (int i = 0; i < size; i++) {
                    if (beamMatrix[x+i, y] != 1) {
                        y++;
                        goto continueOuterLoop;
                    }
                }

                // Loop through y-axis
                for (int i = 0; i < size; i++) {
                    if (beamMatrix[x, y+i] != 1) {
                        x++;
                        goto continueOuterLoop;
                    }
                }

                // If we got here, it means it fit!
                // Hurray!
                break;

                continueOuterLoop:;
            }

            // Let's paint the square!
            for (int i = 0; i < size; i++) for (int j = 0; j < size; j++) beamMatrix[x+i, y+j] = 2;

            // Adjust indicies and break
            x += xDist-margin;
            y += yDist-margin;

            // Print beam (and square)
            Console.WriteLine("The tractor beam looks like (with initial index being ({0}, {1})):", x, y);
            string[] colorPallette = { " ", "█", "#" };
            Console.Write(SpaceImageFormat.ImageToString(beamMatrix, colorPallette));
            Console.WriteLine("The answer is: {0}", x * 10000 + y);
        }

        // If not horizontal => veritcal
        Point2D FindMostOuterPoint<E>(E[,] matrix, Func<E, bool> predicate, bool transpose = false) {
            int width = transpose ? matrix.GetLength(1) : matrix.GetLength(0);
            int height = transpose ? matrix.GetLength(0) : matrix.GetLength(1);

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    int x = transpose ? j : width-1-i;
                    int y = transpose ? width-1-i : j;

                    if (predicate(matrix[x, y])) return new Point2D(x, y);
                }
            }

            return null;
        }

        int[,] ProbeBeamArea(long[] program, int size, int xIndex = 0, int yIndex = 0) {
            // Init computer
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            computer.AddInstruction(new IntcodeInput());
            computer.AddInstruction(new IntcodeOutput());
            computer.AddInstruction(new IntcodeJumpIfTrue());
            computer.AddInstruction(new IntcodeJumpIfFalse());
            computer.AddInstruction(new IntcodeLessThan());
            computer.AddInstruction(new IntcodeEquals());
            computer.AddInstruction(new IntcodeAdjustRelativeBase());

            // Init matrix
            int[,] matrix = new int[size, size];

            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {

                    // Reload program and run
                    computer.LoadProgram(program);
                    computer.Run(new long[] { xIndex+x, yIndex+y });

                    matrix[x, y] = (int) computer.GetOutput();
                }
            }

            return matrix;
        }
    }
}
