using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class AmplificationCircuit : AdventOfCodePuzzle {
        public AmplificationCircuit() : base(2019, 7) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Load program
            long[] program = IntcodeComputer.ParseProgram(puzzleInput);

            // Init amplifiers (computers)
            char[] amplifiers = {'A', 'B', 'C', 'D', 'E'};
            int N = amplifiers.Length;

            IntcodeComputer[] computers = new IntcodeComputer[N];

            for (int i = 0; i < N; i++) {
                IntcodeComputer computer = new IntcodeComputer();
                computer.AddInstruction(new IntcodeAddition());
                computer.AddInstruction(new IntcodeMultiplication());
                computer.AddInstruction(new IntcodeHalt());
                computer.AddInstruction(new IntcodeInput());
                computer.AddInstruction(new IntcodeOutput());
                computer.AddInstruction(new IntcodeJumpIfTrue());
                computer.AddInstruction(new IntcodeJumpIfFalse());
                computer.AddInstruction(new IntcodeLessThan());
                computer.AddInstruction(new IntcodeEquals());
                computers[i] = computer;
            }

            // Calculate phase permutations/combinations
            int[] phaseSettings = new int[N];
            List<List<int>> phaseSequences;

            for (int i = 0; i < N; i++) phaseSettings[i] = i;
            phaseSequences = generateCombinations(new List<int>(phaseSettings));

            // Iterate through
            long maxThrusterSignal;
            maxThrusterSignal = phaseSequences.Select(phaseSequence => {

                long output = 0;
                for (int i = 0; i < N; i++) {
                    IntcodeComputer computer = computers[i];
                    int phase = phaseSequence[i];

                    computer.LoadProgram(program);
                    computer.Run(new long[] {phase, output});
                    
                    output = computer.GetOutput();
                }
                
                return output;
            }).Max();

            Console.WriteLine("The maximal signal that can be sent to the thrusters is: {0}", maxThrusterSignal);

            // Part Two
            // Calculate phase permutations/combinations

            
            maxThrusterSignal = phaseSequences.Select(phaseSequence => {

                // Load program once
                // Add phase input
                for (int i = 0; i < N; i++) {
                    IntcodeComputer computer = computers[i];
                    int phase = 5+phaseSequence[i];

                    computer.LoadProgram(program);
                    computer.AddInput(phase);
                }

                int n = 0;
                long output = 0;
                while (true) {
                    IntcodeComputer computer = computers[n];
                    computer.Run(output);
                    output = computer.GetOutput();

                    n++;
                    if (n == N && computer.IsFinished()) break;
                    n = n % N;
                }
                
                return output;
            }).Max();

            Console.WriteLine("The maximal signal that can be sent to the thrusters is: {0}", maxThrusterSignal);
        }

        public static List<List<int>> generateCombinations(List<int> collection) {
            List<List<int>> combinations = new List<List<int>>();

            if (collection.Count == 1) combinations.Add(collection);
            else foreach (int item in collection) {
                List<int> subcollection = new List<int>(collection);
                subcollection.Remove(item);
                
                List<List<int>> subcombinations = generateCombinations(subcollection);
                foreach (List<int> subcombination in subcombinations) {
                    subcombination.Add(item);
                    combinations.Add(subcombination);
                }
            }

            return combinations;
        }

    }
}
