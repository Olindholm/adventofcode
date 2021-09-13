using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class Point3D {
        
        public static Point3D ORIGIN = new Point3D(0, 0, 0);

        private int X, Y, Z;

        public Point3D(int x, int y, int z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public int GetX() {
            return this.X;
        }

        public int GetY() {
            return this.Y;
        }

        public int GetZ() {
            return this.Z;
        }

        public Point3D Add(Point3D p) {
            int x = this.GetX() + p.GetX();
            int y = this.GetY() + p.GetY();
            int z = this.GetZ() + p.GetZ();

            return new Point3D(x, y, z);
        }
        
        public int GetManhattanDistance(Point3D p) {
            return Math.Abs(GetDeltaX(p)) + Math.Abs(GetDeltaY(p)) + Math.Abs(GetDeltaZ(p));;
        }

        public int GetManhattanSize() {
            return this.GetManhattanDistance(ORIGIN);
        }
        
        public int GetDeltaX(Point3D p) {
            return p.GetX() - this.GetX();
        }
        
        public int GetDeltaY(Point3D p) {
            return p.GetY() - this.GetY();
        }
        
        public int GetDeltaZ(Point3D p) {
            return p.GetZ() - this.GetZ();
        }

        override public string ToString() {
            return "Point3D(" + this.GetX() + ", " + this.GetY() + ", " + this.GetZ() + ")";
        }

        public string ToCSV() {
            return this.GetX() + "," + this.GetY() + "," + this.GetZ() + ",";
        }
    }
}
