using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeLessThan : IntcodeInstruction {
        public IntcodeLessThan() : base(7, 3) {}

        override public void Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            int x = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);
            int y = IntcodeInstruction.GetValue(1, computer, parameters, parameterModes);
            IntcodeInstruction.SetValue(2, computer, parameters, parameterModes, Convert.ToInt32(x < y));
        }
    }
}