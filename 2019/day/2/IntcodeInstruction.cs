using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    abstract class IntcodeInstruction {

        private int Intcode, NumberOfParameters;

        public IntcodeInstruction(int intcode, int numberOfParameters) {
            this.Intcode = intcode;
            this.NumberOfParameters = numberOfParameters;
        }

        //
        // return whether or not successful
        // Success means, the instruction pointer will be incrememented
        //
        public abstract bool Execute(IntcodeComputer computer, int[] parameters, int[] parameterModes);

        public int GetOpcode() {
            return this.Intcode;
        }
        
        public int GetNumberOfParameters() {
            return this.NumberOfParameters;
        }

        public static int GetValue(int parameterIndex, IntcodeComputer computer, int[] parameters, int[] parameterModes) {
            int parameterMode = parameterModes[parameterIndex];
            int value;

            if (parameterMode == 0) { // Position Mode
                int valueIndex = parameters[parameterIndex];
                value = computer.GetProgramValue(valueIndex);
            }
            else if (parameterMode == 1) { // immediate Mode
                value = parameters[parameterIndex];
            }
            else throw new Exception("Unsupported parameter mode: " + parameterMode); // Throw error

            return value;
        }
        
        public static void SetValue(int parameterIndex, IntcodeComputer computer, int[] parameters, int[] parameterModes, int value) {
            int parameterMode = parameterModes[parameterIndex];

            if (parameterMode == 0) { // Position Mode
                int valueIndex = parameters[parameterIndex];
                computer.SetProgramValue(valueIndex, value);
            }
            else throw new Exception("Unsupported parameter mode: " + parameterMode); // Throw error
        }
    }
}