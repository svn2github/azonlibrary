using Azon.Helpers.Annotations;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IUsingConverterClause<TSource> {
        [NotNull]
        IBindingModeWithTargetClause<TTarget> Using<TTarget>([NotNull] IValueConverter<TSource, TTarget> converter);
    }
}