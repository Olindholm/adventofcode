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

        public double GetDistance(Point2D p) {
            return MathExtensions.Pythagoras(GetDeltaX(p), GetDeltaY(p));
        }

        public int GetManhattanDistance(Point2D p) {
            return Math.Abs(GetDeltaX(p)) + Math.Abs(GetDeltaY(p));
        }
        public int GetManhattanSize() {
            return this.GetManhattanDistance(ORIGIN);
        }

        public int GetDeltaX(Point2D p) {
            return p.GetX() - this.GetX();
        }
        
        public int GetDeltaY(Point2D p) {
            return p.GetY() - this.GetY();
        }

        public virtual double GetAngle(Point2D p) {
            return Math.Atan2(this.GetDeltaY(p), this.GetDeltaX(p));
        }

        public Point2D ShiftX(int dx) {
            return new Point2D(GetX()+dx, GetY());
        }
        
        public Point2D ShiftY(int dy) {
            return new Point2D(GetX(), GetY()+dy);
        }

        public Point2D Shift(int direction, int delta) {
            direction = MathExtensions.PositiveModulo(direction, 4);
            delta = (direction < 2) ? delta : -delta;

            return (direction % 2 == 0) ? ShiftY(delta) : ShiftX(delta);
        }


        override public bool Equals(Object obj) {
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            Point2D p = (Point2D) obj;
            if (p.GetX() != this.GetX()) return false;
            if (p.GetY() != this.GetY()) return false;

            return true;
        }

        override public int GetHashCode() {
            return (this.GetX() << 2) ^ this.GetY();
        }

        override public string ToString() {
            return "Point2D(" + this.GetX() + ", " + this.GetY() + ")";
        }
    }
}
