namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IUsingConverterClause {
        IBindingModeClause Using(IValueConverter converter);
    }
}