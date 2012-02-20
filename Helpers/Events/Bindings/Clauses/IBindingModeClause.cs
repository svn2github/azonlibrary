namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingModeClause {
        IBindingTargetClause In(BindingMode mode);
    }
}