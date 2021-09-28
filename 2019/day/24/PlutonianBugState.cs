using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class PlutonianBugState {
        int Size = 5;

        int Level;
        List<char> Layout;
        PlutonianBugState innerState, outerState;
        private PlutonianBugState(IEnumerable<char> layout, int level) {
            Layout = layout.ToList();
            Level = level;
        }

        public PlutonianBugState Next() {
            for (int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    int i = x+y*Size;


                }
            }

            //

            return this;
        }

        private void GetAdjacentBugCount(int x, int y) {
            int center = Size/2;

            // If outer edge
            if (x == 0 || x == Size-1 || y == 0 || y == Size-1) {

            }
            // If inner edge
            else if ( (Math.Abs(x-center) == 1 && y == center) || Math.Abs(y-center) == 1 && x == center) {

            }
            // Otherwise... "normal"
            else {
                
            }

        }

        public int GetBugCount() {
            return Layout.Where(c => c == '#').Count();
        }

        public static PlutonianBugState Parse(string input) {
            return new PlutonianBugState(input.Take(12).Append('?').Concat(input.Skip(13)), 0);
        }
    }
}
