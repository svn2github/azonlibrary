using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingTargetClause<TTarget> {
        void To(Expression<Func<TTarget>> target);
    }
}