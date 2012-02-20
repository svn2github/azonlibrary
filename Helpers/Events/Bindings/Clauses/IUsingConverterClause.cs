namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IUsingConverterClause {
        IBindingModeWithTargetClause Using(IValueConverter converter);
    }
}