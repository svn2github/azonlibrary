using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Extensions;

using Azon.Helpers.Events.Subscriptions.Infos;

namespace Azon.Helpers.Events.Subscriptions.Subscribers {
    internal abstract class BaseSubscribeOnChanging<T, TArgs> : ISubscribeOn, IDisposable
        where TArgs : EventArgs
    {
        protected readonly T entity;
        protected readonly IList<ISubscriptionInfo> infos;

        protected BaseSubscribeOnChanging(T entity) {
            this.infos = new List<ISubscriptionInfo>();
            this.entity = entity;

            this.Subscribe();
        }

        public void Attach(ISubscriptionInfo info) {
            this.infos.Add(info);
        }

        public void Dispose() {
            this.Unsubscribe();
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();

        protected virtual void OnEvent(object sender, TArgs e) {
            lock (this.infos) {
                this.infos.RemoveWhere(info => info.Action.Target == null);
                this.infos.Where(this.Filter(e)).ForEach(this.Handler(e));
            }
        }

        protected virtual Action<ISubscriptionInfo> Handler(TArgs e) {
            return info => info.Action.Target(this.entity, e);
        }

        protected abstract Func<ISubscriptionInfo, bool> Filter(TArgs e);
    }
}