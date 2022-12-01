using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class SatelliteKey {

        private string Name;

        public SatelliteKey(string name) {
            this.Name = name;
        }

        public string GetName() {
            return Name;
        }

        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            SatelliteKey s = (SatelliteKey) obj;
            if (!s.GetName().Equals(this.GetName())) return false;

            return true;
        }

        override public int GetHashCode() {
            return GetName().GetHashCode();
        }

        override public string ToString() {
            return GetName();
        }
    }
}
