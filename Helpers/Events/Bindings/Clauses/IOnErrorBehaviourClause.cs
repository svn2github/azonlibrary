namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IOnErrorBehaviourClause {
        IBindingOptionsClause ThrowingOnBindingErrors();
        IBindingOptionsClause SkippingBindingErrors();
        IBindingOptionsClause NotifyingOnBindingErrors();
    }
}
