using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeEquals : IntcodeInstruction {
        public IntcodeEquals() : base(8, 3) {}

        override public bool Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            int x = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);
            int y = IntcodeInstruction.GetValue(1, computer, parameters, parameterModes);
            IntcodeInstruction.SetValue(2, computer, parameters, parameterModes, Convert.ToInt32(x == y));
            return true;
        }
    }
}