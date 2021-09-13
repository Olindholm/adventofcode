using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SensorBoost : AdventOfCodePuzzle {
        public SensorBoost() : base(2019, 9) {}

        override public void Solve() {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(this.GetPuzzleInput());

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

            // Run BOOST in test mode
            computer.LoadProgram(program);
            computer.Run(1);

            // Part Two
            // Run BOOST in sensor boost mode
            computer.LoadProgram(program);
            computer.Run(2);
        }
    }
}
