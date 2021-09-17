using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    // Standard map tiles
    class UnknownTile : MapTile { public UnknownTile() : base('░') {} }
    class WallTile : MapTile { public WallTile() : base('█') {} }

    class MapTile {

        private char Symbol;

        public MapTile(char symbol) {
            this.Symbol = symbol;
        }

        public virtual char GetSymbol() {
            return Symbol;
        }

    }
}
