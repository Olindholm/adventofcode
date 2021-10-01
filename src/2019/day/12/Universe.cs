using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class Universe {

        int Timestep = 0;
        List<CelestialBody> CelestialBodies = new List<CelestialBody>();

        public Universe(int timestep) {
            this.Timestep = timestep;
        }

        public Universe(CelestialBody[] celestialBodies) {
            AddCelestialBodies(celestialBodies);
        }

        public void AddCelestialBodies(IEnumerable<CelestialBody> celestialBodies) {
            foreach (var celestialBody in celestialBodies) AddCelestialBody(celestialBody);
        }

        public virtual void AddCelestialBody(CelestialBody celestialBody) {
            CelestialBodies.Add(celestialBody.Copy());
        }

        public IEnumerable<CelestialBody> GetCelestialBodies() {
            return CelestialBodies;
        }

        public int GetNumberOfCelestialBodies() {
            return CelestialBodies.Count;
        }

        public bool ContainsCelestialBody(CelestialBody celestialBody) {
            return CelestialBodies.Contains(celestialBody);
        }


        public virtual void Simulate(int steps) {
            for (int i = 0; i < steps; i++) {
                // Apply gravity
                foreach (var celestialBody in CelestialBodies) celestialBody.ApplyGravity(CelestialBodies);

                // Apply velocity
                foreach (var celestialBody in CelestialBodies) celestialBody.ApplyVelocity();

                // Increment time
                Timestep++;
            }
        }

        public int GetTotalEnergy() {
            return this.CelestialBodies.Select(celestialBody => celestialBody.GetTotalEnergy()).Sum();
        }

        public int GetTimestep() {
            return Timestep;
        }

        // immutable
        public ImmutableUniverse GetImmutable() {
            ImmutableUniverse universe = new ImmutableUniverse(this.GetTimestep());
            foreach (var celestialBody in GetCelestialBodies()) universe.AddCelestialBody(celestialBody.GetImmutable());
            universe.Preserve();
            return universe;
        }

        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            Universe u = (Universe) obj;
            if (u.GetNumberOfCelestialBodies() != this.GetNumberOfCelestialBodies()) return false;
            foreach (var celestialBody in u.GetCelestialBodies()) if (!this.ContainsCelestialBody(celestialBody)) return false;

            return true;
        }

        override public int GetHashCode() {
            throw new Exception("Mutable universes don't support hash codes!");

            // Just return something
            // This code will never be reached.
            // Disable warning for Unreachable code (CS0162)
            #pragma warning disable CS0162
            return 0;
            #pragma warning restore CS0162
        }
        
        override public string ToString() {
            string str = String.Format("{0}({1}): [\n", this.GetType().Name, GetTimestep());
            foreach (var body in GetCelestialBodies()) str = str + String.Format("    - {0}\n", body);
            return str + "]";
        }
    }

    class ImmutableUniverse : Universe {

        public ImmutableUniverse(int timestep) : base(timestep) {}

        private bool Immutable = false;
        public void Preserve() {
            Immutable = true;
        }

        override public void AddCelestialBody(CelestialBody celestialBody) {
            if (Immutable) throw new Exception("Immutable objects cannot be modified!");
            base.AddCelestialBody(celestialBody);
        }

        override public void Simulate(int steps) {
            if (Immutable) throw new Exception("Immutable objects cannot be modified!");
            base.Simulate(steps);
        }

        override public int GetHashCode() {
            int hashCode = 0;

            // Calc hash code
            foreach (var celestialBody in GetCelestialBodies()) {
                hashCode = 31*hashCode + celestialBody.GetHashCode();
            }

            return hashCode;
        }
    }
}
