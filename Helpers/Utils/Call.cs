using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Azon.Helpers.Constructs;

namespace Azon.Helpers.Utils {
    public static class Call {
        public static void Generic(Expression<Action> expression, params Type[] typeArgs) {
            Call.Generic(expression.Body as MethodCallExpression, typeArgs);
        }

        public static T Generic<T>(Expression<Func<T>> expression, params Type[] typeArgs) {
            return (T)Call.Generic(expression.Body as MethodCallExpression, typeArgs);
        }

        private static object Generic(MethodCallExpression methodCall, Type[] typeArgs) {
            var method = methodCall.Method.GetGenericMethodDefinition().MakeGenericMethod(typeArgs);
            var args = methodCall.Arguments.Select(ExtractParameter).ToArray();

            return method.Invoke(methodCall.Object, args);
        }

        private static object ExtractParameter(Expression parameter) {
            return Switch.Type<Expression, object>(parameter)
                .When<ConstantExpression>(constant => constant.Value)
                .When<MemberExpression>(
                    member => Switch.Type<MemberInfo, object>(member.Member)
                        .When<FieldInfo>(field => field.GetValue(member))
                        .When<PropertyInfo>(property => property.GetValue(member))
                        .OtherwiseThrow<NotSupportedException>(
                            "Member of type {0} is not supported.", member.Member.GetType()
                        )
                )
                .OtherwiseThrow<NotSupportedException>(
                    "Expression of type {0} is not supported.", parameter.GetType()
                );
        }

        //public static object Generic(Delegate action, Type[] typeArgs, params object[] args) {
        //    var method = action.Generic.GetGenericMethodDefinition().MakeGenericMethod(typeArgs);
        //    return method.Invoke(action.Target, args);
        //}
    }
}