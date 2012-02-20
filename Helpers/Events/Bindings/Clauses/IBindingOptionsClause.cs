namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingOptionsClause : IBindingModeWithTargetClause,
                                             IUsingConverterClause, 
                                             IOnErrorBehaviourClause {}
}