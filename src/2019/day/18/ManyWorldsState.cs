using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class ManyWorldsState {

        List<Point2D> RobotPositions = new List<Point2D>();
        HashSet<char> Keys = new HashSet<char>();

        public ManyWorldsState(IEnumerable<Point2D> robotPositions) {
            RobotPositions.AddRange(robotPositions);
        }

        public ManyWorldsState(IEnumerable<Point2D> robotPositions, IEnumerable<char> keys) : this(robotPositions) {
            Keys.UnionWith(keys);
        }

        public List<Point2D> GetRobotPositions() {
            return RobotPositions;
        }
        public HashSet<char> GetKeys() {
            return Keys;
        }

        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            ManyWorldsState that = (ManyWorldsState) obj;
            if (!that.GetRobotPositions().SequenceEqualsIgnoreOrder(this.GetRobotPositions())) return false;
            if (!that.GetKeys().SetEquals(this.GetKeys())) return false;

            return true;
        }

        override public int GetHashCode() {
            return GetRobotPositions().Select(p => p.GetHashCode()).Sum() + GetKeys().Select(c => c.GetHashCode()).Sum();
        }

        override public string ToString() {
            var robots = String.Join(", ", GetRobotPositions());
            var keys = String.Join("", GetKeys());

            return String.Format("State[ Robots: ( {0} ), Keys: ( {1} ) ]", robots, keys);
        }
    }
}
