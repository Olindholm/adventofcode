using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeComputer {

        private Dictionary<int, IntcodeInstruction> instructions = new Dictionary<int, IntcodeInstruction>();
        private int[] Program;
        private int InstructionPointer;
        private bool Running = false;
        private Queue<int> Input = new Queue<int>();

        public void LoadProgram(int[] program) {
            LoadProgram(program, 0);
        }

        public void LoadProgram(int[] program, int instructionPointer) {
            this.Program = (int[]) program.Clone();
            this.InstructionPointer = instructionPointer;
        }

        public int GetProgramValue(int index) {
            return this.Program[index];
        }

        public void SetProgramValue(int index, int value) {
            this.Program[index] = value;
        }

        public int GetProgramIntcode() {
            return GetProgramValue(GetInstructionPointer());
        }

        public int GetInstructionPointer() {
            return this.InstructionPointer;
        }

        public void SetInstructionPointer(int instructionPointer) {
            this.InstructionPointer = instructionPointer;
        }

        public void IncrementInstructionPointer(int increment) {
            SetInstructionPointer(GetInstructionPointer() + increment);
        }

        public void AddInstruction(IntcodeInstruction instruction) {
            this.instructions[instruction.GetOpcode()] = instruction;
        }

        public int GetInput() {
            if (Input.Count > 0) return Input.Dequeue();

            // Request from terminal
            throw new Exception("This is not impolemnented");
        }

        public void Halt() {
            this.Running = false;
        }

        public void Run() {
            Run(null);
        }

        public void Run(int[] input) {
            if (input != null) foreach (int i in input) Input.Enqueue(i);

            this.Running = true;
            while (this.Running) {
                int intcode = GetProgramIntcode();
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
                    int parameterIndex = this.InstructionPointer+1+j;
                    int parameterModeIndex = j+2;

                    parameters[j] = GetProgramValue(parameterIndex);
                    parameterModes[j] = (parameterModeIndex < intcodeDigits.Length) ? intcodeDigits[parameterModeIndex] : 0;
                }

                // Debug
                //Console.Write("Intcode: " + intcode + ", Opcode: " + opcode + " (" + instruction + ")");
                //for (int j = 0; j < numberOfParameters; j++) Console.Write(", Param " + j + ": " + parameters[j] + ", Mode: " + parameterModes[j]);
                //Console.WriteLine("");

                // Execude instruction
                IncrementInstructionPointer(1 + numberOfParameters);
                instruction.Execute(this, parameters, parameterModes);
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