using System;
using System.Linq;
using System.Linq.Expressions;

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
            var target = ExtractParameter(methodCall.Object);
            var args = methodCall.Arguments.Select(ExtractParameter).ToArray();

            return method.Invoke(target, args);
        }

        private static object ExtractParameter(Expression parameter) {
            if (parameter == null)
                return null;

            return Switch.Type<Expression, object>(parameter)
                         .When<LambdaExpression>(lambda => lambda.Compile().DynamicInvoke())
                         .When<ConstantExpression>(constant => constant.Value)
                         .When<Expression>(expr => Expression.Lambda(expr).Compile().DynamicInvoke())
                         .OtherwiseThrow<NotSupportedException>(
                             "Expression of type {0} is not supported.", parameter.GetType()
                         );
        }
    }
}