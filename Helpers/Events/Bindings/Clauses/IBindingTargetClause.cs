using System;
using System.Linq.Expressions;

using Azon.Helpers.Annotations;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingTargetClause<TTarget> {
        void To([NotNull] Expression<Func<TTarget>> target);
    }
}