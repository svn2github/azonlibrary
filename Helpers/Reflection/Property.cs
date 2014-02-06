using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;
using Azon.Helpers.Constructs;
using Azon.Helpers.Extensions;

namespace Azon.Helpers.Reflection {
    public static class Property {
        #region Hierarchy

        [NotNull]
        public static HierarchicalPropertyInfo Hierarchy<T, TResult>([NotNull] Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy((MemberExpression)reference.Body);
        }

        [NotNull]
        public static HierarchicalPropertyInfo Hierarchy<T>([NotNull] Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Switch.Type<Expression, HierarchicalPropertyInfo>(reference.Body)
                .When<UnaryExpression>(u => Hierarchy((MemberExpression)u.Operand))
                .When<MemberExpression>(Hierarchy)
                .OtherwiseThrow<NotSupportedException>(
                    "Expression of type {0} is not supported.",
                    reference.Body);
        }

        [NotNull]
        public static HierarchicalPropertyInfo Hierarchy<T>([NotNull] Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy((MemberExpression)reference.Body);
        }

        [NotNull]
        public static HierarchicalPropertyInfo Hierarchy([NotNull] MemberExpression member) {
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

        [NotNull]
        public static string Path<T, TResult>([NotNull] Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }

        [NotNull]
        public static string Path<T>([NotNull] Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }

        [NotNull]
        public static string Path<T>([NotNull] Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }

        [NotNull]
        public static string Path([NotNull] Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Path(Hierarchy(reference));
        }

        [NotNull]
        public static string Path([NotNull] HierarchicalPropertyInfo propertyInfo) {
            Require.NotNull(propertyInfo, "propertyInfo");
            return string.Join(".", propertyInfo.Hierarchy.Select(p => p.Name));
        }

        #endregion

        #region Type

        [NotNull]
        public static Type Type<T, TResult>([NotNull] Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }

        [NotNull]
        public static Type Type<T>([NotNull] Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }

        [NotNull]
        public static Type Type<T>([NotNull] Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }

        [NotNull]
        public static Type Type([NotNull] Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Hierarchy(reference).PropertyType;
        }

        #endregion

        #region Name

        [NotNull]
        public static string Name<T, TResult>([NotNull] Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        [NotNull]
        public static string Name<T>([NotNull] Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        [NotNull]
        public static string Name<T>([NotNull] Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        [NotNull]
        public static string Name([NotNull] Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Name(Hierarchy(reference));
        }

        private static string Name(HierarchicalPropertyInfo propertyInfo) {
            return string.Join(".", propertyInfo.Hierarchy.Select(p => p.Name));
        }

        #endregion

        #region Info

        [NotNull]
        public static PropertyInfo Info<T, TResult>([NotNull] Expression<Func<T, TResult>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        [NotNull]
        public static PropertyInfo Info<T>([NotNull] Expression<Func<T, object>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        [NotNull]
        public static PropertyInfo Info<T>([NotNull] Expression<Func<T>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        [NotNull]
        public static PropertyInfo Info([NotNull] Expression<Func<object>> reference) {
            Require.NotNull(reference, "reference");
            return Info(Hierarchy(reference));
        }

        private static PropertyInfo Info([NotNull] HierarchicalPropertyInfo propertyInfo) {
            return propertyInfo.Hierarchy.LastOrDefault();
        }

        #endregion

        #region Get-Set

        public static bool Has([NotNull] object item, [NotNull] string propertyPath) {
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

        [CanBeNull]
        public static object Get([NotNull] object item, [NotNull] string propertyPath) {
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

        [CanBeNull]
        public static object GetOrDefault([NotNull] object item, [NotNull] string propertyPath, object @default = null) {
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

        public static void Set<T>([NotNull] Expression<Func<T>> reference, T value) {
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
