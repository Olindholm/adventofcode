using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class FlawedFrequencyTransmission : AdventOfCodePuzzle {
        public FlawedFrequencyTransmission() : base(2019, 16) {}

        override protected void SolvePuzzle(string puzzleInput) {
            // Parse input
            int length = puzzleInput.Length;
            int[] signal = new int[length];
            for (int i = 0; i < length; i++) signal[i] = Int32.Parse(puzzleInput[i].ToString());

            // Perform the FFT algorithm
            int[] processedSignal = FFT(signal, 100);

            Console.Write("After 100 phases of FFT, the first 8 digits are: ");
            for (int i = 0; i < 8; i++) Console.Write(processedSignal[i]);
            Console.WriteLine("");
        }

        int[] FFT(int[] signal, int phases) {
            bool debug = false;

            // Copy signal array to not edit original
            int[] prevSignal = (int[]) signal.Clone();
            int length = signal.Length;

            // Pattern
            int[] pattern = { 0, 1, 0, -1 };

            for (int phase = 0; phase < phases; phase++) {
                int[] nextSignal = new int[length];

                if (debug) {
                    Console.Write("Input signal: ");
                    foreach (int i in prevSignal) Console.Write(i);
                    Console.WriteLine("\n");
                }

                for (int i = 0; i < length; i++) {
                    for (int j = 0; j < length; j++) {
                        int k = ((1+j) / (1+i)) % pattern.Length;
                        int patternMultipler = pattern[k];
                        nextSignal[i] += prevSignal[j] * patternMultipler;

                        if (debug) {
                            Console.Write("{0}*{1,2}", prevSignal[j], patternMultipler);
                            if (j < length-1) Console.Write(" + ");
                        }
                    }

                    // Deduce to one digit
                    nextSignal[i] = Math.Sign(nextSignal[i]) * nextSignal[i] % 10;

                    if (debug) Console.WriteLine(" = {0}", nextSignal[i]);
                }
                
                if (debug) Console.WriteLine("\n");

                // Move forward (for next iteration)
                prevSignal = nextSignal;
            }

            return prevSignal;
        }
    }
}
