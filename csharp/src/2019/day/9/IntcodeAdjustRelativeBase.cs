using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeAdjustRelativeBase : IntcodeInstruction {
        public IntcodeAdjustRelativeBase() : base(9, 1) {}

        override public bool Execute(IntcodeComputer computer, long[] parameters, int[] parameterModes) {
            long x = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);
            computer.IncrementRelativeBase((int) x);
            return true;
        }
    }
}
