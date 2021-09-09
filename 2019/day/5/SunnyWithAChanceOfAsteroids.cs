using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SunnyWithAChanceOfAsteroids : AdventOfCodePuzzle {
        public SunnyWithAChanceOfAsteroids() : base(2019, 5) {}

        override public void Solve() {
            int[] program = IntcodeComputer.ParseProgram(this.GetPuzzleInput());

            IntcodeComputer computer = InitComputer();
            computer.Run(program);
            
            Console.WriteLine(program[0]);
        }
        
        public static IntcodeComputer InitComputer() {
            IntcodeComputer computer = new IntcodeComputer();
            computer.AddInstruction(new IntcodeAddition());
            computer.AddInstruction(new IntcodeMultiplication());
            computer.AddInstruction(new IntcodeHalt());
            computer.AddInstruction(new IntcodeStoreInput());
            computer.AddInstruction(new IntcodePrintOutput());
            return computer;
        }
    }
}