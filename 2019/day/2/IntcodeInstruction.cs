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
        public abstract bool Execute(IntcodeComputer computer, long[] parameters, int[] parameterModes);

        public int GetOpcode() {
            return this.Intcode;
        }
        
        public int GetNumberOfParameters() {
            return this.NumberOfParameters;
        }

        override public string ToString() {
            return base.ToString().Substring(20);
        }

        public static long GetValue(int parameterIndex, IntcodeComputer computer, long[] parameters, int[] parameterModes) {
            int parameterMode = parameterModes[parameterIndex];
            long value;

            if (parameterMode == 0) { // Position mode
                int valueIndex = (int) parameters[parameterIndex];
                value = computer.GetProgramValue(valueIndex);
            }
            else if (parameterMode == 1) { // Immediate mode
                value = parameters[parameterIndex];
            }
            else if (parameterMode == 2) { // Relative mode
                int valueIndex = (int) parameters[parameterIndex];
                value = computer.GetProgramValue(computer.GetRelativeBase() + valueIndex);
            }
            else throw new Exception("Unsupported parameter mode: " + parameterMode); // Throw error

            return value;
        }
        
        public static void SetValue(int parameterIndex, IntcodeComputer computer, long[] parameters, int[] parameterModes, long value) {
            int parameterMode = parameterModes[parameterIndex];

            if (parameterMode == 0) { // Position Mode
                int valueIndex = (int) parameters[parameterIndex];
                computer.SetProgramValue(valueIndex, value);
            }
            else if (parameterMode == 2) { // Relative mode
                int valueIndex = (int) parameters[parameterIndex];
                computer.SetProgramValue(computer.GetRelativeBase() + valueIndex, value);
            }
            else throw new Exception("Unsupported parameter mode: " + parameterMode); // Throw error
        }
    }
}