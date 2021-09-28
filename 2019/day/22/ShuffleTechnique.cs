using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using static AdventOfCode.OxygenSystem;

namespace AdventOfCode {
    
    abstract class ShuffleTechnique {
        public abstract IEnumerable<T> Shuffle<T>(IEnumerable<T> deck); 
        public abstract long ShuffleIndex(long index, long deckSize); 
        public abstract long UnshuffleIndex(long index, long deckSize); 

        override public string ToString() {
            return GetType().Name;
        }
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

        override public IEnumerable<T> Shuffle<T>(IEnumerable<T> deck) {
            var array = deck.ToArray();
            var length = array.Length;
            T[] array2 = new T[length];

            var index = 0;
            for (int i = 0; i < length; i++) {
                array2[index] = array[i];
                index = MathExtensions.PositiveModulo(index+Increment, length);
            }
            
            return array2;
        }

        override public long ShuffleIndex(long index, long deckSize) {
            return (Increment * index) % deckSize;
        }
        override public long UnshuffleIndex(long index, long deckSize) {
            int i = -1;
            long n;
            do {
                i++;
                n = (index + i * deckSize);
            } while (n % Increment != 0);
            return n / Increment;
        }

        public override string ToString() {
            return base.ToString() + String.Format("( {0} )", Increment);
        }
    }

    class CutShuffle : ShuffleTechnique {

        int Cut;
        public CutShuffle(int cut) {
            Cut = cut;
        }

        override public IEnumerable<T> Shuffle<T>(IEnumerable<T> deck) {
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
        
        override public long ShuffleIndex(long index, long deckSize) {
            return MathExtensions.PositiveModulo(index - Cut, deckSize);
        }
        override public long UnshuffleIndex(long index, long deckSize) {
            return MathExtensions.PositiveModulo(index + Cut, deckSize);
        }

        public override string ToString() {
            return base.ToString() + String.Format("( {0} )", Cut);
        }
    }

    class DealIntoNewStackShuffle : ShuffleTechnique {

        override public IEnumerable<T> Shuffle<T>(IEnumerable<T> deck) {
            return deck.Reverse();
        }
        
        override public long ShuffleIndex(long index, long deckSize) {
            return deckSize-1 - index;
        }
        override public long UnshuffleIndex(long index, long deckSize) {
            return ShuffleIndex(index, deckSize);
        }

    }




}
