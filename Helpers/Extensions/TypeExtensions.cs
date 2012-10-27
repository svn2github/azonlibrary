using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class TypeExtensions {
        public static bool IsGenericDefinedAs(this Type type, Type otherType) {
            Require.NotNull(type, "type");
            Require.NotNull(otherType, "otherType");

            return type.IsGenericType
                && otherType.IsGenericTypeDefinition
                && type.GetGenericTypeDefinition() == otherType;
        }

        public static bool IsAssignableFrom<T>(this Type type) {
            Require.NotNull(type, "type");
            return type.IsAssignableFrom(typeof(T));
        }

        public static bool IsAssignableTo<T>(this Type type) {
            Require.NotNull(type, "type");
            return typeof(T).IsAssignableFrom(type);
        }

        public static IEnumerable<Type> GetHierarchy(this Type type, bool includeSelf = true) {
            Require.NotNull(type, "type");

            if (!includeSelf)
                type = type.BaseType;

            while (type != null) {
                yield return type;

                type = type.BaseType;
            }
        }

        public static IEnumerable<Type> GetInterfaces(this Type type, bool includeSelf = true) {
            Require.NotNull(type, "type");

            if (includeSelf && type.IsInterface)
                yield return type;

            var interfaces = type.GetInterfaces().Sort((left, right) => left.IsAssignableFrom(right));

            foreach (var @interface in interfaces) {
                yield return @interface;
            }
        }

        public static bool Inherits<T>(this Type type) {
            return Inherits(type, typeof(T));
        }

        public static bool Inherits(this Type type, Type parentType) {
            Require.NotNull(type, "type");
            Require.NotNull(parentType, "parentType");

            if (type.IsInterface || parentType.IsInterface)
                return false;

            if (!parentType.IsGenericTypeDefinition)
                return parentType.IsAssignableFrom(type);

            return type.GetHierarchy()
                       .Any(parent => parent.IsGenericDefinedAs(parentType));
        }

        public static bool Implements<T>(this Type type) {
            return Implements(type, typeof(T));
        }

        public static bool Implements(this Type type, Type @interface) {
            Require.NotNull(type, "type");
            Require.NotNull(@interface, "interface");

            if (!@interface.IsInterface)
                return false;

            if (!@interface.IsGenericTypeDefinition)
                return @interface.IsAssignableFrom(type);

            return type.GetInterfaces(includeSelf: true)
                       .Any(i => i.IsGenericDefinedAs(@interface));
        }
    }
}
