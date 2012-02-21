using System;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IOnErrorBehaviourClause<TSource> {
        IBindingOptionsClause<TSource> ThrowingOnBindingErrors();
        IBindingOptionsClause<TSource> SkippingBindingErrors();
        IBindingOptionsClause<TSource> NotifyingOnBindingErrors(Action<BindingException> callback);
    }
}
