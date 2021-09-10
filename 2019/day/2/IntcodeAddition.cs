using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeAddition : IntcodeInstruction {
        public IntcodeAddition() : base(1, 3) {}

        override public bool Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            int x = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);
            int y = IntcodeInstruction.GetValue(1, computer, parameters, parameterModes);
            IntcodeInstruction.SetValue(2, computer, parameters, parameterModes, x + y);
            return true;
        }
    }
}