using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Azon.Helpers.Asserts;
using Azon.Helpers.Constructs;
using Azon.Helpers.Extensions;

namespace Azon.Helpers.Reflection {
    public static class Property {
        #region Hierarchy

        public static HierarchicalPropertyInfo Hierarchy<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy((MemberExpression)reference.Body);
        }

        public static HierarchicalPropertyInfo Hierarchy<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Switch.Type<Expression, HierarchicalPropertyInfo>(reference.Body)
                .When<UnaryExpression>(u => Hierarchy((MemberExpression)u.Operand))
                .When<MemberExpression>(Hierarchy)
                .OtherwiseThrow<NotSupportedException>(
                    "Expression of type {0} is not supported.",
                    reference.Body);
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

        #endregion

        #region Path

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

        public static string Path(Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }

        public static string Path(HierarchicalPropertyInfo propertyInfo) {
            Require.NotNull(propertyInfo, "propertyInfo");
            return string.Join(".", propertyInfo.Hierarchy.Select(p => p.Name));
        }

        #endregion

        #region Type

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

        public static Type Type(Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }

        #endregion

        #region Name

        public static string Name<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        public static string Name<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        public static string Name<T>(Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        public static string Name(Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        private static string Name(HierarchicalPropertyInfo propertyInfo) {
            return string.Join(".", propertyInfo.Hierarchy.Select(p => p.Name));
        }

        #endregion

        #region Info

        public static PropertyInfo Info<T, TResult>(Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        public static PropertyInfo Info<T>(Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        public static PropertyInfo Info<T>(Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        public static PropertyInfo Info(Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        private static PropertyInfo Info(HierarchicalPropertyInfo propertyInfo) {
            return propertyInfo.Hierarchy.LastOrDefault();
        }

        #endregion

        #region Get-Set

        public static bool Has(object item, string propertyPath) {
            Require.NotNull(item, "item");
            Require.NotEmpty(propertyPath, "propertyPath");

            var parts = propertyPath.Split('.');
            var nodeType = item.GetType();

            foreach (var part in parts) {
                var property = nodeType.GetProperty(part);
                if (property == null)
                    return false;

                nodeType = property.PropertyType;
            }

            return true;
        }

        public static object Get(object item, string propertyPath) {
            Require.NotNull(item, "item");
            Require.NotEmpty(propertyPath, "propertyPath");

            var parts = propertyPath.Split('.');
            var node = item;

            foreach (var part in parts) {
                Require.NotNull<NullReferenceException>(node,
                    "Illegal attempt to retrieve value by property path '{0}'" +
                    " chained to a property set to null.",
                    propertyPath
                );

                var property = node.GetType().GetProperty(part);

                Require.NotNull<InvalidOperationException>(property,
                    "Property {0} does not exist on type {1}",
                    part, node.GetType()
                );

                node = property.GetValue(node);
            }

            return node;
        }

        public static object GetOrDefault(object item, string propertyPath, object @default = null) {
            Require.NotNull(item, "item");
            Require.NotEmpty(propertyPath, "propertyPath");

            var parts = propertyPath.Split('.');
            var node = item;

            foreach (var part in parts) {
                if (node == null)
                    return @default;

                var property = node.GetType().GetProperty(part);
                if (property == null)
                    return @default;

                node = property.GetValue(node);
            }

            return node;
        }

        public static void Set<T>(Expression<Func<T>> reference, T value) {
            Require.NotNull(reference, "reference");

            var hierarchy = Hierarchy(reference);
            var root = GetRoot((MemberExpression)reference.Body);

            hierarchy.SetValue(root, value, null);
        }

        private static object GetRoot(MemberExpression member) {
            var next = member;

            while (next != null) {
                var field = next.Member as FieldInfo;
                var constant = next.Expression as ConstantExpression;

                if (field != null && constant != null)
                    return field.GetValue(constant.Value);        

                next = next.Expression as MemberExpression;
            }

            return null;
        }

        #endregion
    }
}
