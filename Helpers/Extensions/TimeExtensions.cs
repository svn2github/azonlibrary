using System;

namespace Azon.Helpers.Extensions {
    public static class TimeExtensions {
        public static TimeSpan Day(this double value) {
            return TimeSpan.FromDays(value);
        }

        public static TimeSpan Days(this double value) {
            return TimeSpan.FromDays(value);
        }

        public static TimeSpan Hour(this double value) {
            return TimeSpan.FromHours(value);
        }

        public static TimeSpan Hours(this double value) {
            return TimeSpan.FromHours(value);
        }

        public static TimeSpan Minute(this double value) {
            return TimeSpan.FromMinutes(value);
        }

        public static TimeSpan Minutes(this double value) {
            return TimeSpan.FromMinutes(value);
        }

        public static TimeSpan Second(this double value) {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan Seconds(this double value) {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan Milliseconds(this double value) {
            return TimeSpan.FromMilliseconds(value);
        }

        public static TimeSpan Day(this int value) {
            return TimeSpan.FromDays(value);
        }

        public static TimeSpan Days(this int value) {
            return TimeSpan.FromDays(value);
        }

        public static TimeSpan Hour(this int value) {
            return TimeSpan.FromHours(value);
        }

        public static TimeSpan Hours(this int value) {
            return TimeSpan.FromHours(value);
        }

        public static TimeSpan Minute(this int value) {
            return TimeSpan.FromMinutes(value);
        }

        public static TimeSpan Minutes(this int value) {
            return TimeSpan.FromMinutes(value);
        }

        public static TimeSpan Second(this int value) {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan Seconds(this int value) {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan Milliseconds(this int value) {
            return TimeSpan.FromMilliseconds(value);
        }

        public static TimeSpan Years(this int value) {
            return TimeSpan.FromDays(value * 365.2425);
        }

        public static DateTime Ago(this TimeSpan value) {
            return DateTime.Now - value;
        }

        public static DateTime Hence(this TimeSpan value) {
            return DateTime.Now + value;
        }

        public static DateTime Before(this TimeSpan value, DateTime date) {
            return date - value;
        }

        public static DateTime After(this TimeSpan value, DateTime date) {
            return date + value;
        }
    }
}
