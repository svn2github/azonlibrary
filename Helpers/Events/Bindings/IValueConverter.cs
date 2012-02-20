namespace Azon.Helpers.Events.Bindings {
    public interface IValueConverter {
        object ConvertTo(object sourceValue);

        object ConvertFrom(object targetValue);
    }
}