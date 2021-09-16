using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class StoichiometryItem {

        private string Compound;
        private long Quantity;

        public StoichiometryItem(string compound, long quantity) {
            this.Compound = compound;
            this.Quantity = quantity;
        }

        public string GetCompound() {
            return Compound;
        }

        public long GetQuantity() {
            return Quantity;
        }
    }
}
