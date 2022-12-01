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

        public static IEnumerable<T> Flatten<T>(this T[,] map) {
            for (int y = 0; y < map.GetLength(1); y++) for (int x = 0; x < map.GetLength(0); x++) yield return map[x, y];
        }

        public static IEnumerable<IEnumerable<T>> ToEnumerable<T>(this T[,] map) {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            for (int y = 0; y < height; y++) {
                T[] array = new T[width];
                for (int x = 0; x < map.GetLength(0); x++) array[x] = map[x, y];
                yield return array;
            }
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

        // https://stackoverflow.com/a/1898744/4255176
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k) {
            return k == 0 ? new[] { new T[0] } : elements.SelectMany((e, i) => elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] {e}).Concat(c)));
        }
    }
}
