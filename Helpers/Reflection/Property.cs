using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Reflection {
    public static class Property {
        public static HierarchicalPropertyInfo Hierarchy<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy((MemberExpression)reference.Body);
        }

        public static HierarchicalPropertyInfo Hierarchy<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy((MemberExpression)reference.Body);
        }

        public static HierarchicalPropertyInfo Hierarchy<T>(Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy((MemberExpression)reference.Body);
        }

        public static HierarchicalPropertyInfo Hierarchy(MemberExpression member) {
            Require.NotNull(member, "member");

            var properties = new Stack<PropertyInfo>();
            var next = member;

            while (next != null) {
                if (next.Member is FieldInfo && next.Expression is ConstantExpression)
                    break;

                var propertyInfo = next.Member.DeclaringType.GetProperty(next.Member.Name)
                                ?? next.Member.DeclaringType.GetProperty(
                                        next.Member.Name, BindingFlags.Instance | BindingFlags.NonPublic);

                properties.Push(propertyInfo);
                next = next.Expression as MemberExpression;
            }

            return new HierarchicalPropertyInfo(properties.ToArray());
        }

        public static string Path<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }
        
        public static string Path<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }

        public static string Path<T>(Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }

        public static string Path(HierarchicalPropertyInfo propertyInfo) {
            Require.NotNull(propertyInfo, "propertyInfo");
            return string.Join(".", propertyInfo.Hierarchy.Select(p => p.Name));
        }

        public static Type Type<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }

        public static Type Type<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }

        public static Type Type<T>(Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }
    }
}
