using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodePrintOutput : IntcodeInstruction {
        public IntcodePrintOutput() : base(4, 1) {}

        override public void Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            int output = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);
            Console.WriteLine("Diagnostic code: " + output);
        }
    }
}