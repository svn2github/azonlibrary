using System;

using Azon.Helpers.Asserts;
using Azon.Helpers.Events.Subscriptions.Infos;

namespace Azon.Helpers.Events.Subscriptions.Clauses {
    internal class CallMethodClause<T> : ICallMethodClause<T> {
        private readonly ObjectSubscription<T> _subscription;
        private readonly SubscriptionInfo _info;

        public CallMethodClause(ObjectSubscription<T> subscription, SubscriptionInfo info) {
            this._subscription = subscription;
            this._info = info;
        }

        public IObjectSubscription<T> Call(Action action) {
            Require.NotNull(action, "action");
            return this.Call(action.Target, (sender, eventArgs) => action());
        }

        public IObjectSubscription<T> Call(Action<T> action) {
            Require.NotNull(action, "action");
            return this.Call(action.Target, (sender, eventArgs) => action(sender));
        }

        public IObjectSubscription<T> Call(Action<T, EventArgs> action) {
            Require.NotNull(action, "action");
            return this.Call(action.Target, action);
        }

        private IObjectSubscription<T> Call(object target, Action<T, EventArgs> action) {
            this._info.SetTarget(target);
            this._info.Action = (sender, eventArgs) => action((T)sender, eventArgs);
            this._subscription.Subscribe(this._info);

            return this._subscription;
        }
    }
}