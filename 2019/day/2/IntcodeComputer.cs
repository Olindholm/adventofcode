using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeComputer {

        private Dictionary<int, IntcodeInstruction> instructions = new Dictionary<int, IntcodeInstruction>();
        private int[] Program;
        private int InstructionPointer;
        private bool Running = false;

        // Exit codes
        // 0 - Successful Halt
        // 1 - Halted, waiting for input
        private int ExitCode = 0;


        // Input/output
        private Queue<int> Inputs = new Queue<int>();
        private Queue<int> Outputs = new Queue<int>();

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

        public void AddInputs(int[] inputs) {
            if (inputs != null) foreach (int input in inputs) AddInput(input);
        }
        
        public void AddInput(int input) {
            Inputs.Enqueue(input);
        }

        public int GetInput() {
            if (Inputs.Count > 0) return Inputs.Dequeue();

            // No Input?
            // Halt and wait for Run again
            Halt(1);

            // Method must return something
            // Anything really, this shouldn't be used
            // Check whether the input is good, by checking
            // if computer is still running/halted
            return Int32.MinValue;
        }
        
        public void AddOutput(int output) {
            Outputs.Enqueue(output);
        }

        public int GetOutput() {
            return Outputs.Dequeue();
        }
        
        public void Halt() {
            Halt(0);
        }

        public void Halt(int exitCode) {
            this.Running = false;
            this.ExitCode = exitCode;
        }

        public bool isFinished() {
            return !isRunning() && this.ExitCode == 0;
        }

        public bool isRunning() {
            return this.Running;
        }

        public void Run() {
            Run(null);
        }

        public void Run(int input) {
            Run(new int[] { input });
        }

        public void Run(int[] inputs) {
            AddInputs(inputs);

            this.Running = true;
            while (isRunning()) {
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
                bool success = instruction.Execute(this, parameters, parameterModes);
                if (success) IncrementInstructionPointer(1 + numberOfParameters);
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