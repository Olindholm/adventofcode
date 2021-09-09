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

        public abstract bool Execute(int[] program, int[] parameters, int[] parameterModes);

        public int GetOpcode() {
            return this.Intcode;
        }
        
        public int GetNumberOfParameters() {
            return this.NumberOfParameters;
        }

        public static int GetValue(int parameterIndex, int[] program, int[] parameters, int[] parameterModes) {
            int parameterMode = parameterModes[parameterIndex];
            int value;

            if (parameterMode == 0) { // Position Mode
                int valueIndex = parameters[parameterIndex];
                value = program[valueIndex];
            }
            else if (parameterMode == 1) { // immediate Mode
                value = parameters[parameterIndex];
            }
            else throw new Exception("Unsupported parameter mode: " + parameterMode); // Throw error

            return value;
        }
        
        public static void SetValue(int parameterIndex, int[] program, int[] parameters, int[] parameterModes, int value) {
            int parameterMode = parameterModes[parameterIndex];

            if (parameterMode == 0) { // Position Mode
                int valueIndex = parameters[parameterIndex];
                program[valueIndex] = value;
            }
            else throw new Exception("Unsupported parameter mode: " + parameterMode); // Throw error
        }
    }
}