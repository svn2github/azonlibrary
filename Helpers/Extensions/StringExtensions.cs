using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Azon.Helpers.Extensions {
    public static class StringExtensions {
        [Pure]
        public static bool IsNullOrEmpty(this string value) {
            return string.IsNullOrEmpty(value);
        }

        [Pure]
        public static bool IsNullOrWhiteSpace(this string value) {
            return string.IsNullOrWhiteSpace(value);
        }

        [Pure]
        public static bool IsNotNullOrEmpty(this string value) {
            return !string.IsNullOrEmpty(value);
        }

        [Pure]
        public static bool IsNotNullOrWhiteSpace(this string value) {
            return !string.IsNullOrWhiteSpace(value);
        }

        [Pure]
        public static string[] Split(this string value, params string[] separator) {
            return value.Split(separator, StringSplitOptions.None);
        }

        [Pure]
        public static bool Contains(this string original, string value, StringComparison comparisonType) {
            return original.IndexOf(value, comparisonType) >= 0;
        }

        [Pure]
        public static string SubstringBefore(this string original, string value) {
            return original.SubstringBefore(original.IndexOf(value));
        }

        [Pure]
        public static string SubstringBefore(this string original, string value, StringComparison comparisonType) {
            return original.SubstringBefore(original.IndexOf(value, comparisonType));
        }

        [Pure]
        public static string SubstringBeforeLast(this string original, string value) {
            return original.SubstringBefore(original.LastIndexOf(value));
        }

        [Pure]
        public static string SubstringBeforeLast(this string original, string value, StringComparison comparisonType) {
            return original.SubstringBefore(original.LastIndexOf(value, comparisonType));
        }

        private static string SubstringBefore(this string original, int index) {
            if (index < 0)
                return original;

            return original.Substring(0, index);
        }

        [Pure]
        public static string SubstringAfter(this string original, string value) {
            return original.SubstringAfter(original.IndexOf(value), value.Length);
        }

        [Pure]
        public static string SubstringAfter(this string original, string value, StringComparison comparisonType) {
            return original.SubstringAfter(original.IndexOf(value, comparisonType), value.Length);
        }

        [Pure]
        public static string SubstringAfterLast(this string original, string value) {
            return original.SubstringAfter(original.LastIndexOf(value), value.Length);
        }

        [Pure]
        public static string SubstringAfterLast(this string original, string value, StringComparison comparisonType) {
            return original.SubstringAfter(original.LastIndexOf(value, comparisonType), value.Length);
        }

        private static string SubstringAfter(this string original, int index, int length) {
            if (index < 0)
                return original;

            index += length;
            return original.Substring(index, original.Length - index);
        }

        [Pure]
        public static string RemoveStart(this string original, string prefix) {
            if (!original.StartsWith(prefix))
                return original;

            return original.Substring(prefix.Length);
        }

        [Pure]
        public static string RemoveEnd(this string original, string suffix) {
            if (!original.EndsWith(suffix))
                return original;

            return original.Substring(0, original.Length - suffix.Length);
        }

        [Pure]
        public static string FormatWith(this string formatString, params object[] args) {
            return string.Format(formatString, args);
        }

        [Pure]
        public static string ToBase64(this string text) {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        [Pure]
        public static string FromBase64(this string text) {
            var bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
