using System.Linq;
using System.Collections.Generic;

namespace System {
    public static class IEnumerableExtensions {

        public static IEnumerable<E> Exclude<E>(this IEnumerable<E> enumerable, E value) {
            return enumerable.Where(e => !e.Equals(value));
        }
        public static IEnumerable<E> Replace<E>(this IEnumerable<E> enumerable, E oldValue, E newValue) {
            return enumerable.Select(value => value.Equals(oldValue) ? newValue : value);
        }

        public static IEnumerable<E> ReplaceIndex<E>(this IEnumerable<E> enumerable, int index, E newValue) {
            // This may be written more efficiently with 2 for loops and yields ?
            return enumerable.Select((value, i) => (i == index) ? newValue : value);
        }

        public static IEnumerable<E> Intertwine<E>(this IEnumerable<E> enumerableA, IEnumerable<E> enumerableB) {
            var arrayA = enumerableA.ToArray();
            var arrayB = enumerableB.ToArray();
            var commonLength = Math.Min(arrayA.Length, arrayB.Length);

            for (int i = 0; i < commonLength; i++) {
                yield return arrayA[i];
                yield return arrayB[i];
            }

            for (int i = commonLength; i < arrayA.Length; i++) yield return arrayA[i];
            for (int i = commonLength; i < arrayB.Length; i++) yield return arrayB[i];
        }
    }
}
