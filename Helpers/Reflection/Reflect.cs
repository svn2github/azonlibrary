using System;
using System.Linq.Expressions;

using Azon.Helpers.Constructs;

namespace Azon.Helpers.Reflection {
    public static class Reflect {
        public static string Name<TResult>(Expression<Func<TResult>> expression) {
            return Reflect.Name(expression.Body);
        }

        public static string Name(Expression<Action> expression) {
            return Reflect.Name(expression.Body);
        }

        private static string Name(Expression expression) {
            return Switch.Type<Expression, string>(expression)
                .When<MethodCallExpression>(method => method.Method.Name)
                .When<MemberExpression>(member => member.Member.Name)
                .OtherwiseThrow<NotSupportedException>("Expression of type {0} is not supported", expression);
        }
    }
}
