using System;
using System.Linq.Expressions;

namespace Azon.Helpers.Events.Bindings.Clauses {
    public interface IBindingTargetClause {
        void To<TTarget>(Expression<Func<TTarget>> target);
    }
}