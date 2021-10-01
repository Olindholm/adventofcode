using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeEquals : IntcodeInstruction {
        public IntcodeEquals() : base(8, 3) {}

        override public bool Execute(IntcodeComputer computer, long[] parameters, int[] parameterModes) {
            long x = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);
            long y = IntcodeInstruction.GetValue(1, computer, parameters, parameterModes);
            IntcodeInstruction.SetValue(2, computer, parameters, parameterModes, Convert.ToInt64(x == y));
            return true;
        }
    }
}