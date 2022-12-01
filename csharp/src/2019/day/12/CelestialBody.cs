using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class CelestialBody {

        string Name;
        Dictionary<int, CelestialDimension> Dimensions = new Dictionary<int, CelestialDimension>();

        public CelestialBody(string name) {
            this.Name = name;
        }

        public string GetName() {
            return this.Name;
        }

        public int GetNumberOfDimensions() {
            return Dimensions.Count;
        }

        public bool HasDimension(int dimension) {
            return Dimensions.ContainsKey(dimension);
        }

        public void AddDimension(int dimension, int position) {
            this.AddDimension(dimension, position, 0);
        }

        public void AddDimension(int dimension, int position, int velocity) {
            this.AddDimension(dimension, new CelestialDimension(position, velocity));
        }

        public virtual void AddDimension(int dimension, CelestialDimension celestialDimension) {
            Dimensions.Add(dimension, celestialDimension);
        }

        public CelestialDimension GetDimension(int dimension) {
            return Dimensions[dimension];
        }

        public IEnumerable<(int Dimension, CelestialDimension CelestialDimension)> GetDimensions() {
            return Dimensions.Select(dimensionEntry => (dimensionEntry.Key, dimensionEntry.Value));
        }

        public void ApplyGravity(IEnumerable<CelestialBody> celestialBodies) {
            foreach (var celestialBody in celestialBodies) ApplyGravity(celestialBody);
        }

        public virtual void ApplyGravity(CelestialBody body) {
            foreach(var dimensionEntry in Dimensions) {
                int dimension = dimensionEntry.Key;

                if (body.HasDimension(dimension)) {
                    CelestialDimension celestialDimension = dimensionEntry.Value;

                    int thisPosition = celestialDimension.GetPosition();
                    int bodyPosition = body.GetDimension(dimension).GetPosition();
                    int deltaPosition = bodyPosition - thisPosition;

                    celestialDimension.AddVelocity(Math.Sign(deltaPosition));
                }
            }
        }

        public virtual void ApplyVelocity() {
            foreach(var celestialDimension in Dimensions.Values) celestialDimension.ApplyVelocity();
        }

        public int GetTotalEnergy() {
            return GetPotentialEnergy() * GetKineticEnergy();
        }

        public int GetPotentialEnergy() {
            return Dimensions.Values.Select(celestialDimension => celestialDimension.GetPotentialEnergy()).Sum();
        }

        public int GetKineticEnergy() {
            return Dimensions.Values.Select(celestialDimension => celestialDimension.GetKineticEnergy()).Sum();
        }

        public virtual CelestialBody Copy() {
            CelestialBody celestialBody = new CelestialBody(this.GetName());
            foreach(var dim in GetDimensions()) celestialBody.AddDimension(dim.Dimension, dim.CelestialDimension.Copy());
            return celestialBody;
        }

        public ImmutableCelestialBody GetImmutable() {
            ImmutableCelestialBody celestialBody = new ImmutableCelestialBody(this.GetName());
            foreach(var dim in GetDimensions()) celestialBody.AddDimension(dim.Dimension, dim.CelestialDimension.GetImmutable());
            celestialBody.Preserve();

            return celestialBody;
        }

        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            CelestialBody b = (CelestialBody) obj;
            if (!b.GetName().Equals(this.GetName())) return false;
            if (b.GetNumberOfDimensions() != this.GetNumberOfDimensions()) return false;
            foreach (var dim in b.GetDimensions()) if (!this.HasDimension(dim.Dimension) || !this.GetDimension(dim.Dimension).Equals(dim.CelestialDimension)) return false;

            return true;
        }

        override public int GetHashCode() {
            throw new Exception("Mutable celestial bodies don't support hash codes!");

            // Just return something
            // This code will never be reached.
            // Disable warning for Unreachable code (CS0162)
            #pragma warning disable CS0162
            return 0;
            #pragma warning restore CS0162
        }

        override public string ToString() {
            string str = String.Format("{0}({1}): [\n", this.GetType().Name, this.GetName());
            foreach (var dim in GetDimensions()) str = str + String.Format("    - Dim: {0} => {1}\n", dim.Dimension, dim.CelestialDimension);
            return str + "]";
        }

    }

    class ImmutableCelestialBody : CelestialBody {

        public ImmutableCelestialBody(string name) : base(name) {}

        private bool Immutable = false;
        public void Preserve() {
            Immutable = true;
        }

        override public void AddDimension(int dimension, CelestialDimension celestialDimension) {
            if (Immutable) throw new Exception("Immutable objects cannot be modified!");
            base.AddDimension(dimension, celestialDimension);
        }

        override public void ApplyGravity(CelestialBody body) {
            if (Immutable) throw new Exception("Immutable objects cannot be modified!");
            base.ApplyGravity(body);
        }

        override public void ApplyVelocity() {
            if (Immutable) throw new Exception("Immutable objects cannot be modified!");
            base.ApplyVelocity();
        }

        override public CelestialBody Copy() {
            return this; // This is immutable, no need to make a copy.
        }

        override public int GetHashCode() {
            int hashCode = 0;

            // Calc hash code
            foreach (var dim in GetDimensions()) {
                hashCode = 31*hashCode + dim.CelestialDimension.GetHashCode();
            }

            return hashCode ^ GetName().GetHashCode();
        }
    }
}
