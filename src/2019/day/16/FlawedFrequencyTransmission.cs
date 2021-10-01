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

            // Part two
            // Brute force?
            int indexLength = 7;
            int messageIndex = signal.Select((n, i) => (int) Math.Pow(10, indexLength-1-i)*n).Take(indexLength).Sum();
            int messageLength = 10_000*length - messageIndex;
            
            int[] messageSignal = new int[messageLength];
            for (int i = 0; i < messageLength; i++) messageSignal[i] = signal[(messageIndex+i) % length];

            for (int phase = 0; phase < 100; phase++) {
                int c = 0;
                for (int i = 0; i < messageLength; i++) {
                    int k = messageLength-1-i;
                    c = (c+messageSignal[k]) % 10;
                    messageSignal[k] = c;
                }
            }

            Console.Write("After 100 phases of FFT, the first 8 digits are: ");
            for (int i = 0; i < 8; i++) Console.Write(messageSignal[i]);
            Console.WriteLine("");
        }

/*
        int[] FFT(int[] signal, int phases, int index = 0) {
            // Copy signal array to not edit original
            int length = signal.Length - index;

            int[] prevSignal = new int[length];
            Array.Copy(signal, index, prevSignal, 0, length);

            // Calculate (stage) breakpoints
            int stage01 = 0;
            int stage12 = Math.Max(index, signal.Length/4) - index;
            int stage23 = Math.Max(index, signal.Length/2) - index;
            int stage34 = length;

            // Phase iteration
            for (int phase = 0; phase < phases; phase++) {
                int[] nextSignal = new int[length];

                // Start with stage 3
                FFTStage3(prevSignal, nextSignal, stage23, stage34);
                
                // Stage 2
                FFTStage3(prevSignal, nextSignal, stage12, stage23);

                prevSignal = nextSignal;
            }

            return prevSignal;
        }
        private void FFTStage3(int[] prevSignal, int[] nextSignal, int startIndex, int finalIndex) {
            int length = finalIndex - startIndex;

            nextSignal[finalIndex-1] = prevSignal[finalIndex-1];
            for (int i = 1; i < length; i++) {
                nextSignal[finalIndex-1-i] = (nextSignal[finalIndex-i] + prevSignal[finalIndex-1-i]) % 10;
            }
        }
        
        private void FFTStage2(int[] prevSignal, int[] nextSignal, int startIndex, int finalIndex) {
            int length = finalIndex - startIndex;
            int helpIndex = 2*finalIndex;

            for (int i = 1; i < length; i++) {
                nextSignal[finalIndex-1-i] = (nextSignal[finalIndex-i] + prevSignal[finalIndex-1-i] - prevSignal[helpIndex-2*i] - prevSignal[helpIndex-2*i-1]) % 10;
            }
        }
*/

        private int[] FFT(int[] signal, int phases) {
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
                    for (int j = i; j < length; j++) {
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
