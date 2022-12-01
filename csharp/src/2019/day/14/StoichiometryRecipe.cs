using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class StoichiometryRecipe {

        StoichiometryBalance Inputs = new StoichiometryBalance();
        StoichiometryItem Output;

        public StoichiometryRecipe(IEnumerable<StoichiometryItem> inputs, StoichiometryItem output) {
            foreach (var input in inputs) AddInput(input);
            this.Output = output;
        }

        private void AddInput(StoichiometryItem input) {
            Inputs.AddItem(input);
        }

        public StoichiometryItem GetOutput() {
            return Output;
        }

        public IEnumerable<StoichiometryItem> GetInputs() {
            return Inputs.GetItems();
        }


    }
}
