using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class StoichiometryItem {

        private string Compound;
        private int Quantity;

        public StoichiometryItem(string compound, int quantity) {
            this.Compound = compound;
            this.Quantity = quantity;
        }

        public string GetCompound() {
            return Compound;
        }

        public int GetQuantity() {
            return Quantity;
        }
    }
}
