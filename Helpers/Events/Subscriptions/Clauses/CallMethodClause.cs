using System;

using Azon.Helpers.Events.Subscriptions.Infos;
using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Subscriptions.Clauses {
    internal class CallMethodClause<T> : ICallMethodClause<T> {
        private readonly ObjectSubscription<T> _subscription;
        private readonly SubscriptionInfo _info;

        public CallMethodClause(ObjectSubscription<T> subscription, SubscriptionInfo info) {
            this._subscription = subscription;
            this._info = info;
        }

        public IObjectSubscription<T> Call(Action action) {
            return this.Call((sender, eventArgs) => action());
        }

        public IObjectSubscription<T> Call(Action<T> action) {
            return this.Call((sender, eventArgs) => action(sender));
        }

        public IObjectSubscription<T> Call(Action<T, EventArgs> action) {
            this._info.Action = new Weak<Action<object, EventArgs>>(
                (sender, eventArgs) => action((T)sender, eventArgs)
            );
            this._subscription.Subscribe(this._info);

            return this._subscription;
        }
    }
}