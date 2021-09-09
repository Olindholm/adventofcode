using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class ProgramAlarm1202 : AdventOfCodePuzzle {
        public ProgramAlarm1202() : base(2019, 2) {}

        override public void Solve() {
            int[] program = IntcodeComputer.ParseProgram(this.GetPuzzleInput());
            program[1] = 12;
            program[2] = 2;

            IntcodeComputer computer = InitComputer();
            computer.Run(program);

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