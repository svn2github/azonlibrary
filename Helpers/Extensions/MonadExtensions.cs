using System;

namespace Azon.Helpers.Extensions {
    public static class MonadExtensions {
        public static TResult With<TInput, TResult>(this TInput value, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (value == null)
                return null;

            return evaluator(value);
        }

        public static TResult Return<TInput, TResult>(
            this TInput value,
            Func<TInput, TResult> evaluator,
            TResult fallbackValue
        )
            where TInput : class 
        {
            if (value == null)
                return fallbackValue;

            return evaluator(value);
        }

        public static TInput If<TInput>(this TInput value, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (value == null)
                return null;

            return evaluator(value) ? value : null;
        }

        public static TInput Unless<TInput>(this TInput value, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (value == null)
                return null;

            return evaluator(value) ? null : value;
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class
        {
            if (o == null)
                return null;

            action(o);
            return o;
        }
    }
}
