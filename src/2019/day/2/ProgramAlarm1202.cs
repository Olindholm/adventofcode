using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class ProgramAlarm1202 : AdventOfCodePuzzle {
        public ProgramAlarm1202() : base(2019, 2) {}

        override protected void SolvePuzzle(string puzzleInput) {
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Init computer
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());

            // Edit program
            program[1] = 12;
            program[2] = 2;

            // Load and run program
            computer.LoadProgram(program);
            computer.Run();

            Console.WriteLine("The value left at position 0 is: {0}", computer.GetProgramValue(0));

            // Part two
        }
    }
}
