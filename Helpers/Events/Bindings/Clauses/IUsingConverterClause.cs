namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IUsingConverterClause<TSource> {
        IBindingModeWithTargetClause<TTarget> Using<TTarget>(IValueConverter<TSource, TTarget> converter);
    }
}