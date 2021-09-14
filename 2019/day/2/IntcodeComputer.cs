using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class IntcodeComputer {

        private Dictionary<int, IntcodeInstruction> instructions = new Dictionary<int, IntcodeInstruction>();
        private List<long> Program;
        private int InstructionPointer;
        private int RelativeBase;
        private bool Running = false;

        // Exit codes
        // 0 - Successful Halt
        // 1 - Halted, waiting for input
        private int ExitCode = 0;


        // Input/output
        private Queue<long> Inputs = new Queue<long>();
        private Queue<long> Outputs = new Queue<long>();

        public void LoadProgram(long[] program) {
            LoadProgram(program, 0, 0);
        }

        public void LoadProgram(long[] program, int instructionPointer, int relativeBase) {
            this.Program = new List<long>(program);
            this.InstructionPointer = instructionPointer;
            this.RelativeBase = relativeBase;
        }

        private void EnsureProgramIndexExist(int index) {
            if (index < 0) throw new Exception("Negative indicies are not allowed!");

            int missingSize = (1+index) - Program.Count;
            if (missingSize > 0) Program.AddRange(new long[missingSize]);
        }

        public long GetProgramValue(int index) {
            EnsureProgramIndexExist(index);
            return Program[index];
        }

        public void SetProgramValue(int index, long value) {
            EnsureProgramIndexExist(index);
            Program[index] = value;
        }

        public long GetProgramIntcode() {
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

        public int GetRelativeBase() {
            return RelativeBase;
        }

        public void SetRelativeBase(int relativeBase) {
            this.RelativeBase = relativeBase;
        }
        
        public void IncrementRelativeBase(int increment) {
            SetRelativeBase(GetRelativeBase() + increment);
        }

        public void AddInstruction(IntcodeInstruction instruction) {
            this.instructions[instruction.GetOpcode()] = instruction;
        }

        public void AddInputs(long[] inputs) {
            if (inputs != null) foreach (long input in inputs) AddInput(input);
        }
        
        public void AddInput(long input) {
            Inputs.Enqueue(input);
        }

        public long GetInput() {
            if (Inputs.Count > 0) return Inputs.Dequeue();

            // No Input?
            // Halt and wait for Run again
            Halt(1);

            // Method must return something
            // Anything really, this shouldn't be used
            // Check whether the input is good, by checking
            // if computer is still running/halted
            return Int64.MinValue;
        }
        
        public void AddOutput(long output) {
            Outputs.Enqueue(output);
        }

        public bool HasMoreOutput() {
            return (Outputs.Count > 0);
        }

        public long GetOutput() {
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

        public void Run(long input) {
            Run(new long[] { input });
        }

        public void Run(long[] inputs) {
            AddInputs(inputs);

            this.Running = true;
            while (isRunning()) {
                long intcode = GetProgramIntcode();
                int[] intcodeDigits = Digits(intcode);

                // Extract instruction
                int opcode = intcodeDigits[0];
                if (intcodeDigits.Length > 1) opcode += 10*intcodeDigits[1];
                IntcodeInstruction instruction = this.instructions[opcode];

                // Extract parameters
                int numberOfParameters = instruction.GetNumberOfParameters();
                long[] parameters = new long[numberOfParameters];
                int[] parameterModes = new int[numberOfParameters];

                for (int j = 0; j < numberOfParameters; j++) {
                    int parameterIndex = this.InstructionPointer+1+j;
                    int parameterModeIndex = j+2;

                    parameters[j] = GetProgramValue(parameterIndex);
                    parameterModes[j] = (parameterModeIndex < intcodeDigits.Length) ? intcodeDigits[parameterModeIndex] : 0;
                }

                // Debug
                //string[] parameterModeNames = {"Absolute mode", "Immidiate mode", "Relative mode"};
                //Console.WriteLine("Intcode: {0,5}, Opcode: {1,2} ({2})", intcode, opcode, instruction);
                //for (int j = 0; j < numberOfParameters; j++)
                //    Console.WriteLine("    - Param {0}: {1,6} (Mode: {2} ({3}))", j, parameters[j], parameterModes[j], parameterModeNames[parameterModes[j]]);

                // Execude instruction
                bool success = instruction.Execute(this, parameters, parameterModes);
                if (success) IncrementInstructionPointer(1 + numberOfParameters);
            }

        }

        public static int[] Digits(long x) {
            int numDigits = (int) Math.Log10(x)+1;

            int[] digits = new int[numDigits];
            for (int i = 0; i < numDigits; i++) {
                digits[i] = (int) (x / Math.Pow(10, i)) % 10;
            }

            return digits;
        }
        
        public static long[] ParseProgram(string input) {
            return input.Split(",").Select(i => Int64.Parse(i)).ToArray();
        }
    }
}