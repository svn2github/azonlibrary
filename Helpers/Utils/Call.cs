using System;
using System.Linq;
using System.Linq.Expressions;

using Azon.Helpers.Annotations;
using Azon.Helpers.Asserts;
using Azon.Helpers.Constructs;

namespace Azon.Helpers.Utils {
    public static class Call {
        public static void Generic([NotNull] Expression<Action> expression, [NotNull] params Type[] typeArgs) {
            Require.NotNull(expression, "expression");
            Require.NotEmpty(typeArgs, "typeArgs");

            Call.Generic(expression.Body as MethodCallExpression, typeArgs);
        }

        public static T Generic<T>([NotNull] Expression<Func<T>> expression, [NotNull] params Type[] typeArgs) {
            Require.NotNull(expression, "expression");
            Require.NotEmpty(typeArgs, "typeArgs");

            return (T)Call.Generic(expression.Body as MethodCallExpression, typeArgs);
        }

        private static object Generic(MethodCallExpression methodCall, Type[] typeArgs) {
            var method = methodCall.Method.GetGenericMethodDefinition().MakeGenericMethod(typeArgs);
            var target = ExtractParameter(methodCall.Object);
            var args = methodCall.Arguments.Select(ExtractParameter).ToArray();

            return method.Invoke(target, args);
        }

        private static object ExtractParameter(Expression parameter) {
            if (parameter == null)
                return null;

            return Switch
                .Type<Expression, object>(parameter)
                    .When<LambdaExpression>(lambda => lambda.Compile().DynamicInvoke())
                    .When<ConstantExpression>(constant => constant.Value)
                    .When<Expression>(expr => Expression.Lambda(expr).Compile().DynamicInvoke())
                    .OtherwiseThrow<NotSupportedException>(
                        "Expression of type {0} is not supported.", parameter.GetType()
                    );
        }
    }
}