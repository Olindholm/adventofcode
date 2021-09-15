using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class Satellite {

        private SatelliteKey Key;
        private Satellite Host = null;

        public Satellite(SatelliteKey key) {
            this.Key = key;
        }

        public Satellite GetHost() {
            return Host;
        }

        public void SetHost(Satellite host) {
            this.Host = host;
        }

        public bool HasHost() {
            return Host != null;
        }

        public bool IsTrueSatellite() {
            return HasHost();
        }

        public SatelliteKey GetKey() {
            return this.Key;
        }

        override public string ToString() {
            return GetKey().ToString();
        }

        public int GetTotalOrbits() {
            if (!IsTrueSatellite()) return 0;
            return 1 + Host.GetTotalOrbits();
        }
    }
}
