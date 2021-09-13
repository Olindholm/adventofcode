using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class EmergencyHullPaintingRobot {

        // Direction, (North, East, South, West) <=> (0, 1, 2, 3)
        private int Direction;
        private Point2D Location;

        public EmergencyHullPaintingRobot() : this(Point2D.ORIGIN) {}
        public EmergencyHullPaintingRobot(Point2D location) : this(location, 0) {}
        public EmergencyHullPaintingRobot(Point2D location, int direction) {
            this.Location = location;
            this.Direction = direction;
        }

        public Point2D GetLocation() {
            return this.Location;
        }

        public void SetLocation(Point2D location) {
            this.Location = location;
        }

        public void TurnLeft() {
            SetDirection(GetDirection()-1);
        }

        public void TurnRight() {
            SetDirection(GetDirection()+1);
        }

        public int GetDirection() {
            return this.Direction;
        }

        public void SetDirection(int direction) {
            this.Direction = MathExtensions.PositiveModulo(direction, 4);
        }

        public void MoveForward(int steps) {
            int direction = GetDirection();

            Func<Point2D, int, Point2D> op;
                 if (direction == 0) op = (p, dy) => new Point2D(p.GetX(), p.GetY()+dy);
            else if (direction == 1) op = (p, dx) => new Point2D(p.GetX()+dx, p.GetY());
            else if (direction == 2) op = (p, dy) => new Point2D(p.GetX(), p.GetY()-dy);
            else if (direction == 3) op = (p, dx) => new Point2D(p.GetX()-dx, p.GetY());
            else throw new Exception("Invalid internal direction: " + direction);

            SetLocation(op(GetLocation(), steps));
        }

    }
}
