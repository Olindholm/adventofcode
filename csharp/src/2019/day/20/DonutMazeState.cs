using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class DonutMazeState {

        Point2D Position;
        int Level;

        public DonutMazeState(Point2D position) : this(position, 0) {}
        public DonutMazeState(Point2D position, int level) {
            Position = position;
            Level = level;
        }

        public Point2D GetPosition() {
            return Position;
        }

        public int GetLevel() {
            return Level;
        }

        public bool IsGroundLevel() {
            return GetLevel() == 0;
        }

        public bool IsOnGoal(Point2D goal) {
            return IsGroundLevel() && GetPosition().Equals(goal);
        }

        override public bool Equals(object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            DonutMazeState that = (DonutMazeState) obj;
            if (that.GetLevel() != this.GetLevel()) return false;
            if (!that.GetPosition().Equals(this.GetPosition())) return false;

            return true;
        }

        // override object.GetHashCode
        public override int GetHashCode() {
            return GetPosition().GetHashCode() + GetLevel();
        }

        override public string ToString() {
            return String.Format("State( Pos: {0}, Lvl: {1} )", GetPosition(), GetLevel());
        }

    }
}
