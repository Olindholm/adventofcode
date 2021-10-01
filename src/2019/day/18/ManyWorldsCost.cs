using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class ManyWorldsCost : IComparable<ManyWorldsCost> {
        
        int Cost;
        HashSet<char> RequiredKeys = new HashSet<char>();

        public ManyWorldsCost(int cost) {
            Cost = cost;
        }

        public ManyWorldsCost(int cost, IEnumerable<char> requiredKeys) : this(cost) {
            RequiredKeys.UnionWith(requiredKeys);
        }

        public int GetCost() {
            return Cost;
        }

        public IEnumerable<char> GetRequiredKeys() {
            return RequiredKeys;
        }

        public bool HasRequiredKeys(IEnumerable<char> keys) {
            return RequiredKeys.IsSubsetOf(keys);
        }

        public int CompareTo(ManyWorldsCost that) {
            return this.GetCost() - that.GetCost();
        }

    }
}
