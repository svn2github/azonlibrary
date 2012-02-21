namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingModeClause<TSource> {
        IBindingTargetClause<TSource> In(BindingMode mode);
    }
}