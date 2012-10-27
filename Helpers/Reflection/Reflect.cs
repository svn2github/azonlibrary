using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

using Azon.Helpers.Asserts;
using Azon.Helpers.Constructs;
using Azon.Helpers.Extensions;

namespace Azon.Helpers.Reflection {
    public static class Reflect {
        #region Name

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

        public static string Name(Expression<Func<object>> reference) {
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

        #endregion

        #region DisplayName

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

        public static string DisplayName(Expression<Func<object>> reference) {
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

        #endregion

        #region ShortName

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

        public static string ShortName<TResult>(Expression<Func<object>> reference) {
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

        #endregion

        #region Info

        public static MemberInfo Info<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Info(reference.Body);
        }

        public static MemberInfo Info<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Info(reference.Body);
        }

        public static MemberInfo Info<TResult>(Expression<Func<TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Info(reference.Body);
        }

        public static MemberInfo Info(Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Info(reference.Body);
        }

        public static MemberInfo Info<T>(Expression<Action<T>> reference) {
            Require.NotNull(reference, "reference");
            return Reflect.Info(reference.Body);
        }

        public static MemberInfo Info(Expression<Action> expression) {
            Require.NotNull(expression, "expression");
            return Reflect.Info(expression.Body);
        }

        private static MemberInfo Info(Expression expression) {
            return GetMember(expression);
        }

        #endregion

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
