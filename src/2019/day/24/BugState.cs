using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class BugState {

        bool[,] Layout;
        private BugState(bool[,] layout) {
            Layout = layout;
        }

        public BugState Next() {
            var width = Layout.GetLength(0);
            var height = Layout.GetLength(1);

            var rect = new Rectangle(width, height);

            var nextLayout = new bool[width, height];
            for (int y = 0; y < height; y++) for (int x = 0; x < width; x++) {
                var thisBug = Layout[x, y];

                Point2D thisPos = new Point2D(x, y);
                var adjacentBugs = Enumerable.Range(0, 4)   // Create 4 directions
                .Select(dir => thisPos.Shift(dir))          // Get the 4 points
                .Where(p => rect.Contains(p))               // Filter out points outside range
                .Select(p => Layout[p.GetX(), p.GetY()])    // Get the bug (whether true/false)
                .Where(bug => bug)                          // Filter out non-bugs
                .Count();                                   // Count them

                var nextBug = thisBug;

                // If there is a bug, but not exaclty one adjacent, it dies
                if (thisBug && adjacentBugs != 1) nextBug = false;

                // Empty tile becomes infested if ajdactent is 1-2 bugs
                else if (!thisBug && adjacentBugs >= 1 && adjacentBugs <= 2) nextBug = true;

                // Otherwise (don't change the value)
                // Though set new value in nextLayout
                nextLayout[x, y] = nextBug;
            }

            return new BugState(nextLayout);
        }

        public int GetBiodiversityRating() {
            return Layout.Flatten().Select((bug, i) => bug ? (int) Math.Pow(2, i) : 0).Sum();
        }

        override public bool Equals(object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            BugState that = (BugState) obj;
            if (!that.Layout.Flatten().SequenceEqual(this.Layout.Flatten())) return false;

            return true;
        }

        override public int GetHashCode() {
            return GetBiodiversityRating();
        }

        override public string ToString() {
            return String.Join("\n", Layout.ToEnumerable().Select(row => String.Join("", row.Select(bug => bug ? '#' : '.'))));
        }

        public static BugState Parse(string input) {
            string[] rows = input.SplitToLines();

            int height = rows.Length;
            int width = input.Length / height;

            bool[,] layout = new bool[width, height];

            for (int y = 0; y < height; y++) for (int x = 0; x < width; x++) {
                char c = rows[y][x];
                bool bug;

                     if (c == '.') bug = false;
                else if (c == '#') bug = true;
                else throw new Exception("Invalid input!");

                layout[x, y] = bug;
            }

            return new BugState(layout);
        }
    }
}
