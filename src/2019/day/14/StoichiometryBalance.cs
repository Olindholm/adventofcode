using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class StoichiometryBalance {

        Dictionary<string, long> Items = new Dictionary<string, long>();

        public void AddItem(StoichiometryItem item) {
            AddItem(item.GetCompound(), item.GetQuantity());
        }

        public void AddItem(string compound, long quantity) {
            // If input compound already exists
            // Merge the inputs (quantity)
            if (Items.ContainsKey(compound)) quantity += Items[compound];

            Items[compound] = quantity;
        }

        public IEnumerable<StoichiometryItem> GetItems() {
            return Items.Select(entry => new StoichiometryItem(entry.Key, entry.Value));
        }

        public IEnumerable<StoichiometryItem> GetNonAbundantItems() {
            return GetItems().Where(item => !item.GetCompound().Equals("ORE"));
        }

        public void Clear() {
            Items.Clear();
        }

        public long GetQuantity(string compound) {
            long quantity = 0;
            Items.TryGetValue(compound, out quantity);
            return quantity;
        }

        public void Exchange(StoichiometryRecipe recipe) {
            Exchange(recipe, 1);
        }

        public void Exchange(StoichiometryRecipe recipe, long multiple) {
            // Remove inputs
            foreach (var inputItem in recipe.GetInputs()) AddItem(inputItem.GetCompound(), -multiple * inputItem.GetQuantity());

            // Add output
            var outputItem = recipe.GetOutput();
            AddItem(outputItem.GetCompound(), multiple * outputItem.GetQuantity());
        }

        override public string ToString() {
            string str = String.Format("{0}: [\n", this.GetType().Name);
            foreach (var item in GetItems()) str += String.Format("{0}: {1}\n", item.GetCompound(), item.GetQuantity());
            return str + "]";
        }
    }
}
