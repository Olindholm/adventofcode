using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeMultiplication : IntcodeInstruction {
        public IntcodeMultiplication() : base(2, 3) {}

        override public bool Execute(int[] program, int[] parameters, int[] parameterModes) {
            int x = IntcodeInstruction.GetValue(0, program, parameters, parameterModes);
            int y = IntcodeInstruction.GetValue(1, program, parameters, parameterModes);
            IntcodeInstruction.SetValue(2, program, parameters, parameterModes, x * y);
            return true;
        }
    }
}