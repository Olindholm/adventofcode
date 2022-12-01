using System;
using System.Reflection;

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

        private string GetPuzzleInput() {
            string puzzleInputFile = "src/" + this.GetYear() + "/day/" + this.GetDay() + "/" + "PuzzleInput.txt";
            return ProcessPuzzleInput(System.IO.File.ReadAllText(puzzleInputFile));
        }

        protected virtual string ProcessPuzzleInput(string puzzleInput) {
            return puzzleInput.Trim();
        }

        public void Solve() {
            Solve(GetPuzzleInput());
        }
        public void Solve(string puzzleInput) {
            Console.WriteLine("--- Day {0,2} ({1,4}): {2} ---", GetDay(), GetYear(), GetType().Name);
            SolvePuzzle(puzzleInput);
            Console.Write("\n\n\n"); // Create some space after (for when multiple solve's are run)
        }

        protected abstract void SolvePuzzle(string puzzleInput);

    }
}
