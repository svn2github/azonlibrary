using System;

using Azon.Helpers.Annotations;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IOnErrorBehaviourClause<TSource> {
        [NotNull]
        IBindingOptionsClause<TSource> ThrowingOnBindingErrors();
        [NotNull]
        IBindingOptionsClause<TSource> SkippingBindingErrors();
        [NotNull]
        IBindingOptionsClause<TSource> NotifyingOnBindingErrors([NotNull] Action<BindingException> callback);
    }
}
