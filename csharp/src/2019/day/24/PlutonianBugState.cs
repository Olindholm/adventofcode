using System;
using System.Linq;
using System.Collections.Generic;

using static System.Math;
using static System.MathExtensions;

namespace AdventOfCode {
    class PlutonianBugState {
        private static int SIZE = 5;
        private static int LENGTH = SIZE*SIZE;
        private static int HALF_LENGTH = SIZE*SIZE/2;
        private static int CENTER = SIZE/2;

        int Level;
        List<char> Layout;
        PlutonianBugState InnerState, OuterState;
        private PlutonianBugState(int level) : this(level, Enumerable.Range(0, LENGTH).Select(i => EmptyChar())) {}
        private PlutonianBugState(int level, IEnumerable<char> layout) {
            Level = level;
            Layout = layout.ToList();

            // Verify size
            if (Layout.Count != LENGTH)
                throw new Exception("Wrong size!?");

            // Verify map
            var allButCenter = Layout.Take(HALF_LENGTH).Concat(Layout.Skip(HALF_LENGTH+1));
            if (!allButCenter.All(c => IsBug(c) || IsEmpty(c)))
                throw new Exception("Invalid chars in map!");

            // Edit middle to contain ?
            Layout[HALF_LENGTH] = '?';
        }

        public PlutonianBugState NextState() {
            var nextState = this.Next();
            NextInner(nextState);
            NextOuter(nextState);
            return nextState;
        }

        private PlutonianBugState Next() {
            var nextLayout = Layout.Select((c, i) => {
                var adjacentBugs = GetAdjacentBugCount(i);

                // If bug, and not exactly one adjacent => die
                if (IsBug(c)) {
                    if (adjacentBugs != 1) return EmptyChar();
                }
                // Empty tile becomes infested if adjactent contains 1-2 bugs
                else if (IsEmpty(c)) {
                    if (adjacentBugs >= 1 && adjacentBugs <= 2) return BugChar();
                }

                // Otherwise, remain
                return c;
            });

            return new PlutonianBugState(Level, nextLayout);
        }

        private PlutonianBugState NextInner() {
            // Next (of this)
            var nextState = this.Next();
            NextInner(nextState);
            return nextState;
        }
        private void NextInner(PlutonianBugState nextState) {
            if (InnerState == null) {
                if (GetBugCount() == 0) return;
                InnerState = new PlutonianBugState(Level+1);
                InnerState.OuterState = this;
            }

            // Next of (inner)
            var nextInnerState = InnerState.NextInner();
            nextInnerState.OuterState = nextState;

            nextState.InnerState = nextInnerState;
        }

        private PlutonianBugState NextOuter() {
            // Next (of this)
            var nextState = this.Next();
            NextOuter(nextState);
            return nextState;
        }
        private void NextOuter(PlutonianBugState nextState) {
            if (OuterState == null) {
                if (GetBugCount() == 0) return;
                OuterState = new PlutonianBugState(Level-1);
                OuterState.InnerState = this;
            }

            // Next of (inner)
            var nextOuterState = OuterState.NextOuter();
            nextOuterState.InnerState = nextState;

            nextState.OuterState = nextOuterState;
        }

        private int GetIndexOf(int x, int y) {
            x = PositiveModulo(x, SIZE);
            y = PositiveModulo(y, SIZE);
            return x+y*SIZE;
        }

        private (int X, int Y) GetCoordsOf(int index) {
            int x = index % SIZE;
            int y = index / SIZE;
            return (x, y);
        }

        private IEnumerable<char> GetColumnTiles(int x) {
            for (int y = 0; y < SIZE; y++) yield return Layout[GetIndexOf(x, y)];
        }
        private IEnumerable<char> GetRowTiles(int y) {
            for (int x = 0; x < SIZE; x++) yield return Layout[GetIndexOf(x, y)];
        }
        private char GetTile(int x, int y) {
            return Layout[GetIndexOf(x, y)];
        }

        private int GetAdjacentBugCount(int index) {
            var coords = GetCoordsOf(index);
            return GetAdjacentBugCount(coords.X, coords.Y);
        }
        private int GetAdjacentBugCount(int x, int y) {
            var tiles = Enumerable.Empty<char>();

            // First
            // Get the tiles from outer or inner states

            // If outer edge
            if (OuterState != null) {
                     if (x == 0)      tiles = tiles.Append(OuterState.GetTile(CENTER-1, CENTER)); // WEST
                else if (x == SIZE-1) tiles = tiles.Append(OuterState.GetTile(CENTER+1, CENTER)); // EAST

                     if (y == 0)      tiles = tiles.Append(OuterState.GetTile(CENTER, CENTER-1)); // NORTH
                else if (y == SIZE-1) tiles = tiles.Append(OuterState.GetTile(CENTER, CENTER+1)); // SOUTH
            }
            // If inner edge
            if (InnerState != null) {
                if (x == CENTER) {
                         if (y == CENTER-1) tiles = tiles.Concat(InnerState.GetRowTiles( 0)); // NORTH
                    else if (y == CENTER+1) tiles = tiles.Concat(InnerState.GetRowTiles(-1)); // SOUTH
                }
                else if (y == CENTER) {
                         if (x == CENTER-1) tiles = tiles.Concat(InnerState.GetColumnTiles( 0)); // WEST
                    else if (x == CENTER+1) tiles = tiles.Concat(InnerState.GetColumnTiles(-1)); // EAST
                }
            }

            // Second
            // Get the tiles from inside
            var thisPos = new Point2D(x, y);

            for (int dir = 0; dir < 4; dir++) {
                var p = thisPos.Shift(dir);

                if (p.GetX() == CENTER && p.GetY() == CENTER) continue; // Todo remove?

                if (p.GetX() >= 0 && p.GetX() < SIZE && p.GetY() >= 0 && p.GetY() < SIZE)
                    tiles = tiles.Append(GetTile(p.GetX(), p.GetY()));
            }

            return tiles.Where(c => IsBug(c)).Count();
        }

        private bool IsBug(char c) {
            return c == BugChar();
        }
        private bool IsEmpty(char c) {
            return c == EmptyChar();
        }

        private static char EmptyChar() {
            return '.';
        }
        private static char BugChar() {
            return '#';
        }

        public int GetBugCount() {
            return Layout.Where(c => IsBug(c)).Count();
        }

        public int GetTotalBugCount() {
            return -GetBugCount() + GetInnerBugCount() + GetOuterBugCount();
        }
        private int GetInnerBugCount() {
            var count = GetBugCount();
            if (InnerState != null) count += InnerState.GetInnerBugCount();
            return count;
        }
        private int GetOuterBugCount() {
            var count = GetBugCount();
            if (OuterState != null) count += OuterState.GetOuterBugCount();
            return count;
        }

        override public string ToString() {
            return String.Join("", Layout.Select((c, i) => c.ToString() + ( (i % SIZE == SIZE-1) ? "\n" : "" )));
        }

        public static PlutonianBugState Parse(string input) {
            return new PlutonianBugState(0, input.Exclude('\n'));
        }
    }
}
