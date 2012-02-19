using System;
using System.Collections.Generic;

namespace Azon.Helpers.Extensions {
    public static class ArrayExtensions {
        public static int IndexOf<T>(this T[] array, T value) {
            return Array.IndexOf(array, value);
        }

        public static int IndexOf<T>(this T[] array, T value, int startIndex) {
            return Array.IndexOf(array, value, startIndex);
        }

        public static int IndexOf<T>(this T[] array, T value, int startIndex, int count) {
            return Array.IndexOf(array, value, startIndex, count);
        }

        public static int LastIndexOf<T>(this T[] array, T value) {
            return Array.LastIndexOf(array, value);
        }

        public static int LastIndexOf<T>(this T[] array, T value, int startIndex) {
            return Array.LastIndexOf(array, value, startIndex);
        }

        public static int LastIndexOf<T>(this T[] array, T value, int startIndex, int count) {
            return Array.LastIndexOf(array, value, startIndex, count);
        }

        public static void Reverse<T>(this T[] array) {
            Array.Reverse(array);
        }

        public static void Reverse<T>(this T[] array, int index, int length) {
            Array.Reverse(array, index, length);
        }

        public static void Sort<T>(this T[] array) {
            Array.Sort(array);
        }

        public static void Sort<T>(this T[] array, Comparison<T> comparison) {
            Array.Sort(array, comparison);
        }

        public static void Sort<T>(this T[] array, IComparer<T> comparer) {
            Array.Sort(array, comparer);
        }

        public static void Sort<T>(this T[] array, int index, int length) {
            Array.Sort(array, index, length);
        }

        public static void Sort<T>(this T[] array, int index, int length, IComparer<T> comparer) {
            Array.Sort(array, index, length, comparer);
        }
    }
}
