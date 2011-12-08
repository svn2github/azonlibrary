using System;
using System.Linq;
using System.Reflection;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class CustomAttributeProviderExtensions {
        public static bool HasAttribute<T>(ICustomAttributeProvider provider, bool inherit = true) 
            where T : Attribute
        {
            return HasAttribute(provider, typeof(T), inherit);
        }

        public static bool HasAttribute(ICustomAttributeProvider provider, Type attributeType, bool inherit = true) {
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
