using System;
using System.Linq.Expressions;

using Azon.Helpers.Annotations;
using Azon.Helpers.Events.Subscriptions.Clauses;

namespace Azon.Helpers.Events.Subscriptions {
    public interface IObjectSubscription<T> : ISubscription {
        [NotNull]
        ICallMethodClause<T> HasChanged([NotNull] Expression<Func<T, object>> reference);
        [NotNull]
        ICallMethodClause<T> IsChanging([NotNull] Expression<Func<T, object>> reference);
    }
}