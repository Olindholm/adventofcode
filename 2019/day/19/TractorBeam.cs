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
            int width = 50;
            int height = 50;

            int[,] affectorMatrix = new int[width, height];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    // Reload program and run
                    computer.LoadProgram(program);
                    computer.Run(new long[] { x, y });

                    affectorMatrix[x, y] = (int) computer.GetOutput();
                }
            }

            int numberOfAffectedPoints = 0;
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) if (affectorMatrix[x, y] == 1) numberOfAffectedPoints++;

            Console.WriteLine("The tractor beam looks like:");
            Console.Write(SpaceImageFormat.ImageToString(affectorMatrix));
            Console.WriteLine("The number of affected points within the 50x50 area is: {0}", numberOfAffectedPoints);
        }
    }
}
