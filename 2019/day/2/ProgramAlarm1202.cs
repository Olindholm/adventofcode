using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class ProgramAlarm1202 : AdventOfCodePuzzle {
        public ProgramAlarm1202() : base(2019, 2) {}

        override protected void SolvePuzzle(string puzzleInput) {
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);
            program[1] = 12;
            program[2] = 2;

            IntcodeComputer computer = InitComputer();
            computer.LoadProgram(program);
            computer.Run();

            Console.WriteLine(program[0]);
        }

        public static IntcodeComputer InitComputer() {
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            return computer;
        }
    }
}