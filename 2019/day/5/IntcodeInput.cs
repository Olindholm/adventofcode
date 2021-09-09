using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeInput : IntcodeInstruction {
        public IntcodeInput() : base(3, 1) {}

        override public void Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            int input = computer.GetInput();
            IntcodeInstruction.SetValue(0, computer, parameters, parameterModes, input);
        }
    }
}