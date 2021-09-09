using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeComputer {

        private Dictionary<int, IntcodeInstruction> instructions;

        public IntcodeComputer() {
            this.instructions = new Dictionary<int, IntcodeInstruction>();
        }

        public void AddInstruction(IntcodeInstruction instruction) {
            this.instructions[instruction.GetOpcode()] = instruction;
        }

        public void Run(int[] program) {

            int i = 0;
            while (true) {
                int intcode = program[i];
                int[] intcodeDigits = Digits(intcode);

                // Extract instruction
                int opcode = intcodeDigits[0];
                if (intcodeDigits.Length > 1) opcode += 10*intcodeDigits[1];
                IntcodeInstruction instruction = this.instructions[opcode];

                // Extract parameters
                int numberOfParameters = instruction.GetNumberOfParameters();
                int[] parameters = new int[numberOfParameters];
                int[] parameterModes = new int[numberOfParameters];

                for (int j = 0; j < numberOfParameters; j++) {
                    int parameterIndex = i+1+j;
                    int parameterModeIndex = j+2;

                    parameters[j] = program[parameterIndex];
                    parameterModes[j] = (parameterModeIndex < intcodeDigits.Length) ? intcodeDigits[parameterModeIndex] : 0;
                }

                // Debug
                //Console.Write("Intcode: " + intcode + ", Opcode: " + opcode + " (" + instruction + ")");
                //for (int j = 0; j < numberOfParameters; j++) Console.Write(", Param " + j + ": " + parameters[j] + ", Mode: " + parameterModes[j]);
                //Console.WriteLine("");

                // Execude instruction
                bool nohalt = instruction.Execute(program, parameters, parameterModes);
                i += 1 + numberOfParameters;

                if (!nohalt) break;
            }

        }

        public static int[] Digits(int x) {
            int numDigits = (int) Math.Log10(x)+1;

            int[] digits = new int[numDigits];
            for (int i = 0; i < numDigits; i++) {
                digits[i] = (int) (x / Math.Pow(10, i)) % 10;
            }

            return digits;
        }
        
        public static int[] ParseProgram(string input) {
            return input.Split(",").Select(i => Int32.Parse(i)).ToArray();
        }
    }
}