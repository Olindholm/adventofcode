using System.Linq;
using System.Collections.Generic;

namespace System {
    public static class ListExtensions {
        public static bool SequenceEqualsIgnoreOrder<E>(this List<E> listA, List<E> listB) {
            return (listA.Count == listB.Count) && !listA.Except(listB).Any();
        }
    }
}
