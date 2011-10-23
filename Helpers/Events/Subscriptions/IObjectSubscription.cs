using System;
using System.Linq.Expressions;

using Azon.Helpers.Events.Subscriptions.Clauses;

namespace Azon.Helpers.Events.Subscriptions {
    public interface IObjectSubscription<T> : ISubscription {
        ICallMethodClause<T> HasChanged(Expression<Func<T, object>> reference);
        ICallMethodClause<T> IsChanging(Expression<Func<T, object>> reference);
    }
}