using System;

namespace Azon.Helpers.Events.Subscriptions.Clauses {
    public interface ICallMethodClause<T> {
        IObjectSubscription<T> Call(Action action);
        IObjectSubscription<T> Call(Action<T> action);
        IObjectSubscription<T> Call(Action<T, EventArgs> action);
    }
}