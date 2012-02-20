using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingModeWithTargetClause : IBindingModeClause, IBindingTargetClause {
        void OneWayFrom<TTarget>(Expression<Func<TTarget>> target);
        void OneWayTo<TTarget>(Expression<Func<TTarget>> target);
    }
}