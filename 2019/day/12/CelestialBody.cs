using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class CelestialBody {

        private Point3D Position, Velocity;

        public CelestialBody(int x, int y, int z) {
            this.Position = new Point3D(x, y, z);
            this.Velocity = new Point3D(0, 0, 0);
        }

        public Point3D GetPosition() {
            return this.Position;
        }

        public void AddPosition(Point3D v) {
            this.Position = GetPosition().Add(v);
        }

        public Point3D GetVelocity() {
            return this.Velocity;
        }

        public void AddVelocity(Point3D dv) {
            this.Velocity = GetVelocity().Add(dv);
        }

        public void ApplyVelocity() {
            AddPosition(GetVelocity());
        }

        public void ApplyGravity(List<CelestialBody> bodies) {
            foreach (var body in bodies) ApplyGravity(body);
        }

        public void ApplyGravity(CelestialBody body) {
            Point3D thisPosition = this.GetPosition();
            Point3D bodyPosition = body.GetPosition();

            int dx = thisPosition.GetDeltaX(bodyPosition);
            int dy = thisPosition.GetDeltaY(bodyPosition);
            int dz = thisPosition.GetDeltaZ(bodyPosition);

            int ddx = 0;
            int ddy = 0;
            int ddz = 0;

            if (thisPosition.GetX() != bodyPosition.GetX()) ddx = (thisPosition.GetX() < bodyPosition.GetX()) ? 1 : -1;
            if (thisPosition.GetY() != bodyPosition.GetY()) ddy = (thisPosition.GetY() < bodyPosition.GetY()) ? 1 : -1;
            if (thisPosition.GetZ() != bodyPosition.GetZ()) ddz = (thisPosition.GetZ() < bodyPosition.GetZ()) ? 1 : -1;

            AddVelocity(new Point3D(ddx, ddy, ddz));
        }

        public int GetTotalEnergy() {
            return GetPotentialEnergy() * GetKineticEnergy();
        }
        
        public int GetPotentialEnergy() {
            return GetPosition().GetManhattanSize();
        }

        public int GetKineticEnergy() {
            return GetVelocity().GetManhattanSize();
        }

        override public string ToString() {
            return "CelestialBody(Pos: " + GetPosition() + ", Vel: " + GetVelocity() + ")";
        }
        
        public string ToCSV() {
            return GetPosition().ToCSV() + GetVelocity().ToCSV();
        }

    }
}
