using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class Rectangle {

        Point2D P1, P2;
        public Rectangle(Point2D p1, Point2D p2) {
            P1 = p1;
            P2 = p2;
        }
        public Rectangle(Point2D p2) : this(Point2D.ORIGIN, p2) {}
        public Rectangle(int width, int height) : this(new Point2D(width, height)) {}

        public bool Contains(Point2D p) {
            var xInside = p.GetX() >= P1.GetX() && p.GetX() < P2.GetX();
            var yInside = p.GetY() >= P1.GetY() && p.GetY() < P2.GetY();

            return xInside && yInside;
        }
    }
}
