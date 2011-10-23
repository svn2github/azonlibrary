using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

using AshMind.Extensions;

using Azon.Helpers.Events.Subscriptions.Clauses;
using Azon.Helpers.Events.Subscriptions.Infos;
using Azon.Helpers.Events.Subscriptions.Subscribers;
using Azon.Helpers.Reflection;

namespace Azon.Helpers.Events.Subscriptions {
    internal class ObjectSubscription<T> : BaseSubscription, IObjectSubscription<T> {
        private static readonly IDictionary<Type, Func<T, ISubscribeOn>> _createSubscriber =
            new Dictionary<Type, Func<T, ISubscribeOn>> {
                { typeof(ChangedSubscriptionInfo), entity => new SubscribeOnChanged((INotifyPropertyChanged)entity) },
                { typeof(ChangingSubscriptionInfo), entity => new SubscribeOnChanging((INotifyPropertyChanging)entity) }
            };

        private readonly IList<ISubscriptionInfo> _infos = new List<ISubscriptionInfo>();
        private readonly IDictionary<Type, ISubscribeOn> _subscribers = new Dictionary<Type, ISubscribeOn>();
        private readonly AggregateSubscription _subscription;
        private readonly T _entity;

        public ObjectSubscription(AggregateSubscription subscription, T entity) {
            this._subscription = subscription;
            this._entity = entity;
        }

        public ICallMethodClause<T> HasChanged(Expression<Func<T, object>> reference) {
            var info = new ChangedSubscriptionInfo(this._entity, Property.Name(reference));
            this._infos.Add(info);
            return new CallMethodClause<T>(this, info);
        }

        public ICallMethodClause<T> IsChanging(Expression<Func<T, object>> reference) {
            var info = new ChangingSubscriptionInfo(this._entity, Property.Name(reference));
            this._infos.Add(info);
            return new CallMethodClause<T>(this, info);
        }

        public override IObjectSubscription<TEntity> Object<TEntity>(TEntity entity) {
            return this._subscription.Object(entity);
        }

        protected override void UnsubscribeCore() {
            this._subscribers.Values.ForEach(subscriber => subscriber.Unsubscribe());
            this._subscription.Unsubscribe();
        }

        internal void Subscribe(ISubscriptionInfo info) {
            this.GetOrCreateSubscriber(info).Attach(info);
        }

        protected ISubscribeOn GetOrCreateSubscriber(ISubscriptionInfo info) {
            var type = info.GetType();

            return this._subscribers.GetValueOrDefault(type)
                ?? (this._subscribers[type] = _createSubscriber[type](this._entity));
        }
    }
}