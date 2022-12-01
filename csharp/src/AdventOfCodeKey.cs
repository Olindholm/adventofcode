using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class AdventOfCodeKey : IComparable<AdventOfCodeKey> {

        private int Year, Day;

        public AdventOfCodeKey(int year, int day) {
            this.Year = year;
            this.Day = day;
        }

        public int GetYear() {
            return this.Year;
        }
        public int GetDay() {
            return this.Day;
        }


        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            AdventOfCodeKey d = (AdventOfCodeKey) obj;
            if (d.GetYear() != this.GetYear()) return false;
            if (d.GetDay() != this.GetDay()) return false;

            return true;
        }

        override public int GetHashCode() {
            return (GetYear() << 2) ^ GetDay();
        }

        override public string ToString() {
            return String.Format("{0}/{1}", GetDay(), GetYear());
        }

        public int CompareTo(AdventOfCodeKey key) {
            int dYear = this.GetYear() - key.GetYear();
            if (dYear != 0) return dYear;

            return this.GetDay() - key.GetDay();
        }

    }
}
