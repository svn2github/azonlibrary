using System.Collections.Generic;

using AshMind.Extensions;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Events.Subscriptions {
    internal class AggregateSubscription : BaseSubscription {
        private readonly IList<ISubscription> _subscriptions = new List<ISubscription>();

        public override IObjectSubscription<TEntity> Object<TEntity>(TEntity entity) {
            Require.NotNull(entity, "entity");

            var subscription = new ObjectSubscription<TEntity>(this, entity);
            this._subscriptions.Add(subscription);
            return subscription;
        }

        protected override void UnsubscribeCore() {
            this._subscriptions.ForEach(subscription => subscription.Unsubscribe());
        }
    }
}
