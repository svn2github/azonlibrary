using System;
using System.Diagnostics;
using System.Globalization;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;

namespace Azon.Helpers.Extensions {
    public static class FormattableExtensions {
        [DebuggerStepThrough]
        public static string ToString([NotNull] this IFormattable value, [CanBeNull] IFormatProvider provider) {
            Require.NotNull(value, "value");

            return value.ToString(null, provider);
        }

        [DebuggerStepThrough]
        public static string ToInvariantString([NotNull] this IFormattable value) {
            Require.NotNull(value, "value");
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
