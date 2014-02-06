using System;
using System.Linq.Expressions;

using Azon.Helpers.Annotations;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingModeWithTargetClause<TTarget> : IBindingModeClause<TTarget>,
                                                             IBindingTargetClause<TTarget>
    {
        void OneWayFrom([NotNull] Expression<Func<TTarget>> target);
        void OneWayTo([NotNull] Expression<Func<TTarget>> target);
    }
}