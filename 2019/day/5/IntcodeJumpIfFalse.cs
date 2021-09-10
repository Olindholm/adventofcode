using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeJumpIfFalse : IntcodeInstruction {
        public IntcodeJumpIfFalse() : base(6, 2) {}

        override public bool Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            int x = IntcodeInstruction.GetValue(0, computer, parameters, parameterModes);

            if (x == 0) {
                int instructionPointer = IntcodeInstruction.GetValue(1, computer, parameters, parameterModes);
                computer.SetInstructionPointer(instructionPointer);
                return false;
            }
            
            return true;
        }
    }
}