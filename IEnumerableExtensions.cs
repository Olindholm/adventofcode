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
    }
}
