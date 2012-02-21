namespace Azon.Helpers.Events.Bindings {
    public interface IValueConverter<TSource, TTarget> {
        TTarget ConvertTo(TSource sourceValue);

        TSource ConvertFrom(TTarget targetValue);
    }
}