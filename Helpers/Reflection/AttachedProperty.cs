using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Azon.Helpers.Extensions;

namespace Azon.Helpers.Reflection {
    public static class AttachedProperty {
        private static readonly ConditionalWeakTable<object, Dictionary<string, object>> _store = new ConditionalWeakTable<object, Dictionary<string, object>>();

        public static T Get<T>(object target, string propertyName) {
            var value = (Dictionary<string, object>)null;
            var found = _store.TryGetValue(target, out value);
            if (!found)
                return default(T);

            return (T)value.GetValueOrDefault(propertyName, (object)default(T));
        }

        public static void Set<T, TValue>(T target, string propertyName, TValue value) {
            var properties = _store.GetOrCreateValue(target);
            properties[propertyName] = value;
        }
    }
}