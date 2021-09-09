using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeStoreInput : IntcodeInstruction {
        public IntcodeStoreInput() : base(3, 1) {}

        override public bool Execute(int[] program, int[] parameters, int[] parameterModes) {
            // Get input
            int input = 1;

            IntcodeInstruction.SetValue(0, program, parameters, parameterModes, input);
            return true;
        }
    }
}