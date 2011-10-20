using System;

namespace Azon.Helpers.Extensions {
    public static class TypeExtensions {
        public static bool IsGenericDefinedAs(this Type type, Type otherType) {
            Require.NotNull(type, "type");
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == otherType;
        }
    }
}
