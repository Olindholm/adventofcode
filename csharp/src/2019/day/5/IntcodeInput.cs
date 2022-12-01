using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeInput : IntcodeInstruction {
        public IntcodeInput() : base(3, 1) {}

        override public bool Execute(IntcodeComputer computer, long[] parameters, int[] parameterModes) {
            long input = computer.GetInput();
            if (!computer.IsRunning()) return false;

            IntcodeInstruction.SetValue(0, computer, parameters, parameterModes, input);
            return true;
        }
    }
}
