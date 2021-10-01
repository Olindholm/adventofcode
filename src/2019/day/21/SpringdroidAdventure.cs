using System;
using System.Linq;
using System.Collections.Generic;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    
    class SpringdroidAdventure : AdventOfCodePuzzle {
        public SpringdroidAdventure() : base(2019, 21) {}

        override protected void SolvePuzzle(string puzzleInput) {
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Part one
            // Create springscript
            string[] springscript = {
                "NOT C J",
                "AND D J",
                "NOT A T",
                "OR T J",
            };
            RunSpringdroid(program, springscript, "WALK");

            // Part two
            // Create springscript
            string[] extendedSpringscript = {
                "NOT B J",

                "NOT C T",
                "AND H T",
            
                "OR T J",
                "AND D J",

                "NOT A T",
                "OR T J",
            };
            RunSpringdroid(program, extendedSpringscript, "RUN");
        }

        void RunSpringdroid(long[] program, string[] springscript, string mode) {
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

            // Load springscript (input)
            var input = springscript.Append(mode).Aggregate("", (acc, str) => acc += str + "\n").Select(c => (long) c);
            computer.AddInputs(input);

            // Run springscript (computer)
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
