using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    // Standard robot tiles
    class RobotTile : OccupiableTile {
        public RobotTile() : base(' ', 'O') {}
        public RobotTile(bool occupied) : base(' ', 'O', occupied) {}
    }
    class OxygenSystemTile : OccupiableTile { public OxygenSystemTile() : base('/', 'Ø') {} }


    class OccupiableTile : MapTile {

        private char OccupiedSymbol;
        private bool Occupied;

        public OccupiableTile(char symbol, char occupiedSymbol) : this(symbol, occupiedSymbol, false) {}
        
        public OccupiableTile(char symbol, char occupiedSymbol, bool occupied) : base(symbol) {
            this.OccupiedSymbol = occupiedSymbol;
            this.Occupied = occupied;
        }

        public bool IsOccupied() {
            return Occupied;
        }

        override public char GetSymbol() {
            return IsOccupied() ? OccupiedSymbol : base.GetSymbol();
        }

        public void SetOccupied(bool occupied) {
            this.Occupied = occupied;
        }

        public void Occupy() {
            SetOccupied(true);
        }

        public void Vacate() {
            SetOccupied(false);
        }

    }
}
