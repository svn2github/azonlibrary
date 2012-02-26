using System;
using System.Linq;
using System.Reflection;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class CustomAttributeProviderExtensions {
        public static T GetAttribute<T>(this ICustomAttributeProvider provider, bool inherit = true)
            where T : Attribute
        {
            Require.NotNull(provider, "provider");
            return (T)provider.GetCustomAttributes(typeof(T), inherit).SingleOrDefault();
        }

        public static T[] GetAttributes<T>(this ICustomAttributeProvider provider, bool inherit = true)
            where T : Attribute 
        {
            Require.NotNull(provider, "provider");
            var attrs = provider.GetCustomAttributes(typeof(T), inherit);
            return attrs.Cast<T>().ToArray();
        }

        public static bool HasAttribute<T>(this ICustomAttributeProvider provider, bool inherit = true) 
            where T : Attribute
        {
            return HasAttribute(provider, typeof(T), inherit);
        }

        public static bool HasAttribute(this ICustomAttributeProvider provider, Type attributeType, bool inherit = true) {
            Require.NotNull(provider, "provider");
            Require.NotNull(attributeType, "attributeType");
            Require.That(
                attributeType.Inherits<Attribute>(),
                "Given type {0} is not inherited from {1}.",
                attributeType, typeof(Attribute)
            );

            return provider.GetCustomAttributes(attributeType, inherit).Any();
        }
    }
}
