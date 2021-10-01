using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeOutput : IntcodeInstruction {
        public IntcodeOutput() : base(4, 1) {}

        override public bool Execute(IntcodeComputer computer, long[] parameters, int[] parameterModes) {
            long output = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);
            computer.AddOutput(output);
            return true;
        }
    }
}