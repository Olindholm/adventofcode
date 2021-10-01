using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class VacuumRobotInstruction {

        private int Direction, Steps;

        public VacuumRobotInstruction(int direction, int steps) {
            // Direction
            //  0 - Forward
            //  1 - Right
            // -1 - Left (also 2)

            SetDirection(direction);
            this.Steps = steps;
        }

        public void SetDirection(int direction) {
            this.Direction = MathExtensions.PositiveModulo(direction, 3);
        }

        public int GetDirection() {
            return Direction;
        }

        public string GetLetterDirection() {
            string[] letters = { "", "R", "L" };
            return letters[GetDirection()];
        }

        public int GetSteps() {
            return Steps;
        }

        override public string ToString() {
            return GetLetterDirection() + GetSteps();
        }

        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            VacuumRobotInstruction d = (VacuumRobotInstruction) obj;
            if (d.GetDirection() != this.GetDirection()) return false;
            if (d.GetSteps() != this.GetSteps()) return false;
            
            return true;
        }
        
        override public int GetHashCode() {
            return (this.GetDirection() << 2) ^ this.GetSteps();
        }
    }
}
