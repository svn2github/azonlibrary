using System;
using System.Collections.Generic;

using Azon.Helpers.Annotations;

namespace Azon.Helpers.Extensions {
    public static class DictionaryExtensions {
        [CanBeNull]
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key) {
            return dictionary.GetValueOrDefault(key, default(TValue));
        }

        [CanBeNull]
        public static TDefault GetValueOrDefault<TKey, TValue, TDefault>(
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TDefault @default
        ) 
            where TValue : TDefault 
        {
            TValue local;
            return dictionary.TryGetValue(key, out local) ? local : @default;
        }

        [NotNull]
        public static TValue GetOrCreate<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new() 
        {
            return dictionary.GetOrCreate(key, () => new TValue());
        }

        [NotNull]
        public static TValue GetOrCreate<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            TKey key,
            [NotNull, InstantHandle] Func<TValue> valueFactory
        ) {
            TValue value;
            if (!dictionary.TryGetValue(key, out value)) {
                value = valueFactory();
                dictionary[key] = value;
            }
            return value;
        }
    }
}
