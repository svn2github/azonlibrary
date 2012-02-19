using System.Collections.Generic;

namespace Azon.Helpers.Extensions {
    public static class DictionaryExtensions {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
            return dictionary.GetValueOrDefault(key, default(TValue));
        }

        public static TDefault GetValueOrDefault<TKey, TValue, TDefault>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TDefault @default
        ) 
            where TValue : TDefault 
        {
            TValue local;
            return dictionary.TryGetValue(key, out local) ? local : @default;
        }
    }
}
