using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class CarePackage : AdventOfCodePuzzle {
        public CarePackage() : base(2019, 13) {}

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

            // Load program
            computer.LoadProgram(program);
            computer.Run();

            Dictionary<Point2D, int> screen = new Dictionary<Point2D, int>();
            while (computer.HasMoreOutput()) {
                // Get 3 ouputs
                int x = (int) computer.GetOutput();
                int y = (int) computer.GetOutput();
                int c = (int) computer.GetOutput();

                screen[new Point2D(x, y)] = c;
            }

            // Count blocks
            int numberOfBlocks = screen.Values.Where(c => (c == 2)).Count();
            Console.WriteLine(numberOfBlocks);
        }
    }
}
