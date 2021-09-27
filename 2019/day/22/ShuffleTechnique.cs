using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    
    interface ShuffleTechnique {
        IEnumerable<T> Shuffle<T>(IEnumerable<T> deck); 
    }

    static class ShuffleTechniqueFactory {
        public static ShuffleTechnique ParseShuffleTechnique(string str) {
            // ^deal with increment ([0-9]+)$
            // ^cut (-?[0-9]+)$
            // ^deal into new stack$
            Match match;

            match = Regex.Match(str, "^deal with increment ([0-9]+)$");
            if (match.Success) return new DealWithIncrementShuffle(Int32.Parse(match.Groups[1].ToString()));

            match = Regex.Match(str, "^cut (-?[0-9]+)$");
            if (match.Success) return new CutShuffle(Int32.Parse(match.Groups[1].ToString()));

            match = Regex.Match(str, "^deal into new stack$");
            if (match.Success) return new DealIntoNewStackShuffle();

            throw new Exception("Could not parse shuffle technique!");

            // Just return something
            // This code will never be reached.
            // Disable warning for Unreachable code (CS0162)
            #pragma warning disable CS0162
            return null;
            #pragma warning restore CS0162
        }
    }

    class DealWithIncrementShuffle : ShuffleTechnique {

        int Increment;
        public DealWithIncrementShuffle(int increment) {
            Increment = increment;
        }
        
        public int GetIncrement() {
            return Increment;
        }

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> deck) {
            var array = deck.ToArray();
            var length = array.Length;

            var index = 0;
            for (int i = 0; i < length; i++) {
                yield return array[index];
                index = MathExtensions.PositiveModulo(index-Increment, length);
            }
        }

    }

    class CutShuffle : ShuffleTechnique {

        int Cut;
        public CutShuffle(int cut) {
            Cut = cut;
        }

        public int GetCut() {
            return Cut;
        }

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> deck) {
            IEnumerable<T> first;
            IEnumerable<T> second;

            if (Cut > 0) {
                first = deck.Skip(Cut);
                second = deck.Take(Cut);
            }
            else {
                first = deck.Reverse().Take(-Cut).Reverse();
                second = deck.Reverse().Skip(-Cut).Reverse();
            }

            return first.Concat(second);
        }
    }

    class DealIntoNewStackShuffle : ShuffleTechnique {

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> deck) {
            return deck.Reverse();
        }
    }




}
