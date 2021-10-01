using System.Collections.Generic;

namespace System {
    public static class DictionaryExtensions {
        public static E GetValueOrDefault<K, E>(this Dictionary<K, E> dict, K key, E value) {
            if (!dict.ContainsKey(key)) dict[key] = value;
            return dict[key];
        }
    }
}
