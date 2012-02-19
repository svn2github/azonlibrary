using System;
using System.Diagnostics;
using System.Globalization;

namespace Azon.Helpers.Extensions {
    public static class FormattableExtensions {
        [DebuggerStepThrough]
        public static string ToString(this IFormattable value, IFormatProvider provider) {
            return value.ToString(null, provider);
        }

        [DebuggerStepThrough]
        public static string ToInvariantString(this IFormattable value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
