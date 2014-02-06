using System.Reflection;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class PropertyInfoExtensions {
        public static object GetValue([NotNull] this PropertyInfo property, object obj) {
            Require.NotNull(property, "property");
            return property.GetValue(obj, null);
        }

        public static void SetValue([NotNull] this PropertyInfo property, object obj, object value) {
            Require.NotNull(property, "property");
            property.SetValue(obj, value, null);
        }
    }
}
