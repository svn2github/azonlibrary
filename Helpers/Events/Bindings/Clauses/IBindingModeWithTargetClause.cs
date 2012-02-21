using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingModeWithTargetClause<TTarget> : IBindingModeClause<TTarget>,
                                                             IBindingTargetClause<TTarget>
    {
        void OneWayFrom(Expression<Func<TTarget>> target);
        void OneWayTo(Expression<Func<TTarget>> target);
    }
}