using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class Point2D {

        public static Point2D ORIGIN = new Point2D(0, 0);

        private int X, Y;

        public Point2D(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public int GetX() {
            return this.X;
        }
        
        public int GetY() {
            return this.Y;
        }

        public int GetManhattanDistance(Point2D p) {
            return Math.Abs(this.GetX() - p.GetX()) + Math.Abs(this.GetY() - p.GetY());
        }
        public int GetManhattanSize() {
            return this.GetManhattanDistance(ORIGIN);
        }


        public override bool Equals(Object obj) {
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            Point2D p = (Point2D) obj;
            if (p.GetX() != this.GetX()) return false;
            if (p.GetY() != this.GetY()) return false;

            return true;
        }

        public override int GetHashCode() {
            return (this.GetX() << 2) ^ this.GetY();
        }

        public override string ToString() {
            return "Point2D(" + this.GetX() + ", " + this.GetY() + ")";
        }
    }
}
