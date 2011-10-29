using System;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class TypeExtensions {
        public static bool IsGenericDefinedAs(this Type type, Type otherType) {
            Require.NotNull(type, "type");
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == otherType;
        }

        public static bool IsAssignableFrom<T>(this Type type) {
            Require.NotNull(type, "type");
            return type.IsAssignableFrom(typeof(T));
        }
    }
}
