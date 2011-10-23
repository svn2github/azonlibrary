using System;
using System.ComponentModel;

using Azon.Helpers.Events.Subscriptions.Infos;

namespace Azon.Helpers.Events.Subscriptions.Subscribers {
    internal class SubscribeOnChanging : BaseSubscribeOnChanging<INotifyPropertyChanging, PropertyChangingEventArgs> {
        public SubscribeOnChanging(INotifyPropertyChanging entity) : base(entity) {}

        public override void Subscribe() {
            this.entity.PropertyChanging += this.OnEvent;
        }

        public override void Unsubscribe() {
            this.entity.PropertyChanging -= this.OnEvent;
        }

        protected override Func<ISubscriptionInfo, bool> Filter(PropertyChangingEventArgs e) {
            return info => info.PropertyName == e.PropertyName;
        }
    }
}