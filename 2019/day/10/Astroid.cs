using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class Astroid : Point2D {
        public Astroid(int x, int y) : base(x, y) {}

        override public double GetAngle(Point2D p) {
            double angle = PositiveModulo(Math.PI/2 + base.GetAngle(p), 2*Math.PI);
            //Console.WriteLine("This: " + this + ", P: " + p + ", Angle: " + 180/Math.PI*angle);
            return angle;
        }

        private static double PositiveModulo(double n, double m) {
            return (n % m + m) % m;
        }
    }
}
