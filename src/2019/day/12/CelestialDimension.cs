using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class CelestialDimension {

        private int Position, Velocity;

        public CelestialDimension(int position) : this(position, 0) {}
        public CelestialDimension(int position, int velocity) {
            this.Position = position;
            this.Velocity = velocity;
        }

        public int GetPosition() {
            return this.Position;
        }

        public virtual void SetPosition(int position) {
            this.Position = position;
        }

        public void AddPosition(int dPosition) {
            SetPosition(GetPosition() + dPosition);
        }

        public int GetVelocity() {
            return this.Velocity;
        }

        public virtual void SetVelocity(int velocity) {
            this.Velocity = velocity;
        }

        public void AddVelocity(int dVelocity) {
            SetVelocity(GetVelocity() + dVelocity);
        }

        public void ApplyVelocity() {
            AddPosition(GetVelocity());
        }
        
        public int GetPotentialEnergy() {
            return Math.Abs(GetPosition());
        }

        public int GetKineticEnergy() {
            return Math.Abs(GetVelocity());
        }
        
        public virtual CelestialDimension Copy() {
            return new CelestialDimension(GetPosition(), GetVelocity());
        }

        public ImmutableCelestialDimension GetImmutable() {
            return new ImmutableCelestialDimension(GetPosition(), GetVelocity());
        }

        override public bool Equals(Object obj) {
            if (obj == this) return true; // If same reference => same object
            if (obj == null) return false;
            if (!obj.GetType().IsInstanceOfType(this)) return false;

            CelestialDimension d = (CelestialDimension) obj;
            if (d.GetPosition() != this.GetPosition()) return false;
            if (d.GetVelocity() != this.GetVelocity()) return false;
            
            return true;
        }

        override public int GetHashCode() {
            throw new Exception("Mutable celestial dimensions don't support hash codes!");
            
            // Just return something
            // This code will never be reached.
            // Disable warning for Unreachable code (CS0162)
            #pragma warning disable CS0162
            return 0;
            #pragma warning restore CS0162
        }

        override public string ToString() {
            return String.Format("{0}(Pos: {1,4}, Vel: {2,4})", this.GetType().Name, GetPosition(), GetVelocity());
        }

    }

    class ImmutableCelestialDimension : CelestialDimension {

        public ImmutableCelestialDimension(int position, int velocity) : base(position, velocity) {}

        override public void SetPosition(int position) {
            throw new Exception("Immutable objects cannot be modified!");
        }
        
        override public void SetVelocity(int velocity) {
            throw new Exception("Immutable objects cannot be modified!");
        }
        
        override public CelestialDimension Copy() {
            return this; // This is immutable, no need to make a copy.
        }

        override public int GetHashCode() {
            return (GetPosition() << 2) ^ GetVelocity();
        }
    }
}
