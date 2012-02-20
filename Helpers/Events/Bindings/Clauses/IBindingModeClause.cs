using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingModeClause {
        void OneWayFrom<TTarget>(Expression<Func<TTarget>> target);
        void OneWayTo<TTarget>(Expression<Func<TTarget>> target);
        void To<TTarget>(Expression<Func<TTarget>> target);
    }
}