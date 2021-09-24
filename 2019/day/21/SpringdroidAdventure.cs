using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    
    class SpringdroidAdventure : AdventOfCodePuzzle {
        public SpringdroidAdventure() : base(2019, 21) {}

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

            // Load program
            computer.LoadProgram(program);

            // Create springscript and load it (input)
            string[] cmds = {
                "NOT C J",
                "AND D J",
                "NOT A T",
                "OR T J",
            };
            var input = cmds.Append("WALK").Aggregate("", (acc, str) => acc += str + "\n").Select(c => (long) c);
            computer.AddInputs(input);

            // Run computer
            computer.Run();

            // Get output
            var output = new List<long>();
            while (computer.HasMoreOutput()) output.Add(computer.GetOutput());

            if (output.Count != 34) { // You f'd up
                foreach(var n in output) Console.Write((char) n);
                return;
            }

            // Otherwise!, correct!
            long hullDamage = output.Last();
            Console.WriteLine("The amount of hull damage reported is: {0}", hullDamage);
        }
    }
}
