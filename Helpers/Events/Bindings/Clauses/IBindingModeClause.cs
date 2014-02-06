using Azon.Helpers.Annotations;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingModeClause<TSource> {
        [NotNull]
        IBindingTargetClause<TSource> In(BindingMode mode);
    }
}