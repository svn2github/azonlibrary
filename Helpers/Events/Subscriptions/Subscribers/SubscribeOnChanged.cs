using System;
using System.ComponentModel;

using Azon.Helpers.Events.Subscriptions.Infos;

namespace Azon.Helpers.Events.Subscriptions.Subscribers {
    internal class SubscribeOnChanged : BaseSubscribeOnChanging<INotifyPropertyChanged, PropertyChangedEventArgs> {
        public SubscribeOnChanged(INotifyPropertyChanged entity) : base(entity) {}

        public override void Subscribe() {
            this.entity.PropertyChanged += this.OnEvent;
        }

        public override void Unsubscribe() {
            this.entity.PropertyChanged -= this.OnEvent;
        }

        protected override Func<ISubscriptionInfo, bool> Filter(PropertyChangedEventArgs e) {
            return info => info.PropertyName == e.PropertyName;
        }
    }
}