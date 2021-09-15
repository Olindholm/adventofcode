using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    abstract class AdventOfCodePuzzle {

        private AdventOfCodeKey Key;

        public AdventOfCodePuzzle(int year, int day) {
            this.Key = new AdventOfCodeKey(year, day);
        }

        public AdventOfCodeKey GetKey() {
            return this.Key;
        }

        public int GetYear() {
            return this.Key.GetYear();
        }
        public int GetDay() {
            return this.Key.GetDay();
        }

        public string GetPuzzleInput() {
            string puzzleInputFile = this.GetYear() + "/day/" + this.GetDay() + "/" + "PuzzleInput.txt";
            return System.IO.File.ReadAllText(puzzleInputFile).Trim();
        }
        
        public abstract void Solve();

    }
}
