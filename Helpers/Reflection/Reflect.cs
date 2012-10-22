using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

using Azon.Helpers.Asserts;
using Azon.Helpers.Constructs;
using Azon.Helpers.Extensions;

namespace Azon.Helpers.Reflection {
    public static class Reflect {
        public static string Name<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Name(reference.Body);
        }

        public static string Name<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Name(reference.Body);
        }

        public static string Name<TResult>(Expression<Func<TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Name(reference.Body);
        }

        public static string Name<T>(Expression<Action<T>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Name(reference.Body);
        }

        public static string Name(Expression<Action> expression) {
            Require.NotNull(expression, "expression");
            return Reflect.Name(expression.Body);
        }

        private static string Name(Expression expression) {
            return GetMember(expression).Name;
        }

        public static string DisplayName<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.DisplayName(reference.Body);
        }

        public static string DisplayName<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.DisplayName(reference.Body);
        }

        public static string DisplayName<TResult>(Expression<Func<TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.DisplayName(reference.Body);
        }

        public static string DisplayName<T>(Expression<Action<T>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.DisplayName(reference.Body);
        }

        public static string DisplayName(Expression<Action> expression) {
            Require.NotNull(expression, "expression");
            return Reflect.DisplayName(expression.Body);
        }

        private static string DisplayName(Expression expression) {
            var member = GetMember(expression);
            var displayAttribute = member.GetAttribute<DisplayAttribute>();
            if (displayAttribute == null)
                return member.Name;

            return displayAttribute.Name ?? member.Name;
        }

        public static string ShortName<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.ShortName(reference.Body);
        }

        public static string ShortName<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.ShortName(reference.Body);
        }

        public static string ShortName<TResult>(Expression<Func<TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.ShortName(reference.Body);
        }

        public static string ShortName<T>(Expression<Action<T>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.ShortName(reference.Body);
        }

        public static string ShortName(Expression<Action> expression) {
            Require.NotNull(expression, "expression");
            return Reflect.ShortName(expression.Body);
        }

        private static string ShortName(Expression expression) {
            var member = GetMember(expression);
            var displayAttribute = member.GetAttribute<DisplayAttribute>();
            if (displayAttribute == null)
                return member.Name;

            return displayAttribute.ShortName ?? member.Name;
        }

        private static MemberInfo GetMember(Expression expression) {
            return Switch.Type<Expression, MemberInfo>(expression)
                .When<MethodCallExpression>(m => m.Method)
                .When<MemberExpression>(m => m.Member)
                .OtherwiseThrow<NotSupportedException>(
                    "Expression of type {0} is not supported", 
                    expression);
        }
    }
}
