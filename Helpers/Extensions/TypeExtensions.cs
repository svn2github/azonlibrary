using System;
using System.Collections.Generic;
using System.Linq;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class TypeExtensions {
        public static bool IsGenericDefinedAs(this Type type, Type template, bool recursive = false) {
            Require.NotNull(type, "type");
            Require.NotNull(template, "template");

            if (!template.IsGenericTypeDefinition)
                return false;

            var toTest = template.IsInterface
                ? type.GetInterfaces(includeSelf: true)
                : type.GetHierarchy(includeSelf: true);

            foreach (var test in toTest) {
                if (test.IsGenericType && test.GetGenericTypeDefinition() == template)
                    return true;

                if (!recursive)
                    return false;
            }

            return false;
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

        /// <summary>
        /// Returns the first not dynamically generated type in hierarchy of
        /// the given type starting with itself.
        /// </summary>
        /// <param name="type">A type to test.</param>
        /// <returns>Not dynamically generated type.</returns>
        public static Type Real(this Type type) {
            Require.NotNull(type, "type");

            while (type.Assembly.IsDynamic) {
                type = type.BaseType;
            }

            return type;
        }

        public static Type[] GetGenericArgumentsOf(this Type type, Type template) {
            Require.NotNull(type, "type");
            Require.NotNull(template, "template");
            Require.That(template.IsGenericTypeDefinition, "template");

            var toTest = template.IsInterface
                ? type.GetInterfaces(includeSelf: true)
                : type.GetHierarchy(includeSelf: true);

            var constructedFrom = toTest.FirstOrDefault(
                t => t.IsGenericType
                  && t.GetGenericTypeDefinition() == template
            );

            return constructedFrom == null
                ? new Type[0]
                : constructedFrom.GetGenericArguments();
        }
    }
}
