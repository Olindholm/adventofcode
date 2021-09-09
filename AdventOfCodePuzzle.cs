using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    abstract class AdventOfCodePuzzle {

        private int Year, Day;

        public AdventOfCodePuzzle(int year, int day) {
            this.Year = year;
            this.Day = day;
        }

        public int GetYear() {
            return this.Year;
        }
        public int GetDay() {
            return this.Day;
        }

        public string GetPuzzleInput() {
            string puzzleInputFile = this.GetYear() + "/day/" + this.GetDay() + "/" + "PuzzleInput.txt";
            return System.IO.File.ReadAllText(puzzleInputFile).Trim();
        }
        
        public abstract void Solve();

    }
}
