using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SunnyWithAChanceOfAsteroids : AdventOfCodePuzzle {
        public SunnyWithAChanceOfAsteroids() : base(2019, 5) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);
            long diagnosticCode = 0;

            // Init computer
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            computer.AddInstruction(new IntcodeInput());
            computer.AddInstruction(new IntcodeOutput());

            // Run program
            computer.LoadProgram(program);
            computer.Run(1);

            // Extract (final) diagnostic code
            while (computer.HasMoreOutput()) diagnosticCode = computer.GetOutput();
            Console.WriteLine("The final diagnostic code is: {0}", diagnosticCode);

            // Part Two
            // Add additional instructions
            computer.AddInstruction(new IntcodeJumpIfTrue());
            computer.AddInstruction(new IntcodeJumpIfFalse());
            computer.AddInstruction(new IntcodeLessThan());
            computer.AddInstruction(new IntcodeEquals());

            // Run program
            computer.LoadProgram(program);
            computer.Run(5);

            // Extract (final) diagnostic code
            while (computer.HasMoreOutput()) diagnosticCode = computer.GetOutput();
            Console.WriteLine("The final diagnostic code is: {0}", diagnosticCode);

        }
    }
}
