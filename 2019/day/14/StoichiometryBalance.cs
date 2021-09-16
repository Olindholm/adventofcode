using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class StoichiometryBalance {

        Dictionary<string, int> Items = new Dictionary<string, int>();

        public void AddItem(StoichiometryItem item) {
            AddItem(item.GetCompound(), item.GetQuantity());
        }

        public void AddItem(string compound, int quantity) {
            // If input compound already exists
            // Merge the inputs (quantity)
            if (Items.ContainsKey(compound)) quantity += Items[compound];

            Items[compound] = quantity;
        }

        public IEnumerable<StoichiometryItem> GetItems() {
            return Items.Select(entry => new StoichiometryItem(entry.Key, entry.Value));
        }

        public int GetQuantity(string compound) {
            int quantity = 0;
            Items.TryGetValue(compound, out quantity);
            return quantity;
        }

        public void Exchange(StoichiometryRecipe recipe) {
            // Remove inputs
            foreach (var inputItem in recipe.GetInputs()) AddItem(inputItem.GetCompound(), -inputItem.GetQuantity());

            // Add output
            var outputItem = recipe.GetOutput();
            AddItem(outputItem.GetCompound(), outputItem.GetQuantity());
        }

        override public string ToString() {
            string str = String.Format("{0}: [\n", this.GetType().Name);
            foreach (var item in GetItems()) str += String.Format("{0}: {1}\n", item.GetCompound(), item.GetQuantity());
            return str + "]";
        }
    }
}
