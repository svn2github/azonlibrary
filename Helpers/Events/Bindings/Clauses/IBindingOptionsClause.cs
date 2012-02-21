namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingOptionsClause<TSource> : IBindingModeWithTargetClause<TSource>,
                                                      IUsingConverterClause<TSource>, 
                                                      IOnErrorBehaviourClause<TSource> {}
}