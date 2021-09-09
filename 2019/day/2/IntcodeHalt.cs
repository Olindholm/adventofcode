using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeHalt : IntcodeInstruction {
        public IntcodeHalt() : base(99, 0) {}

        override public void Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            computer.Halt();
        }
    }
}