using System;

using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Subscriptions.Infos {
    internal abstract class SubscriptionInfo : ISubscriptionInfo {
        protected SubscriptionInfo(object entity, string propertyName) {
            this.Object = entity;
            this.PropertyName = propertyName;
        }

        public Weak<Action<object, EventArgs>> Action { get; set; }
        public object Object { get; set; }
        public string PropertyName { get; set; }
    }
}
