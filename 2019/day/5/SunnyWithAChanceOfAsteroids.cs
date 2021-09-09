using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SunnyWithAChanceOfAsteroids : AdventOfCodePuzzle {
        public SunnyWithAChanceOfAsteroids() : base(2019, 5) {}

        override public void Solve() {
            // Load program
            int[] program = IntcodeComputer.ParseProgram(this.GetPuzzleInput());

            // Init computer
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            computer.AddInstruction(new IntcodeInput());
            computer.AddInstruction(new IntcodePrintOutput());

            // Run program
            computer.LoadProgram(program);
            computer.Run(new int[] {1});

            // Part Two
            // Add additional instructions
            computer.AddInstruction(new IntcodeJumpIfTrue());
            computer.AddInstruction(new IntcodeJumpIfFalse());
            computer.AddInstruction(new IntcodeLessThan());
            computer.AddInstruction(new IntcodeEquals());

            // Run program
            computer.LoadProgram(program);
            computer.Run(new int[] {5});

        }
    }
}