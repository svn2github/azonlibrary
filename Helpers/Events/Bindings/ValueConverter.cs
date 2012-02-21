namespace Azon.Helpers.Events.Bindings {
    internal class ValueConverter<TSource, TTarget> : IValueConverter<TSource, TTarget> {
        public TTarget ConvertTo(TSource sourceValue) {
            return (TTarget)(object)sourceValue;
        }

        public TSource ConvertFrom(TTarget targetValue) {
            return (TSource)(object)targetValue;
        }
    }
}