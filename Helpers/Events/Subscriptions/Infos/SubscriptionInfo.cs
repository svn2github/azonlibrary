using System;

namespace Azon.Helpers.Events.Subscriptions.Infos {
    internal abstract class SubscriptionInfo : ISubscriptionInfo {
        private WeakReference _source;
        private WeakReference _target;

        protected SubscriptionInfo(object source, string propertyName) {
            _source = new WeakReference(source);

            this.PropertyName = propertyName;
        }

        internal void SetTarget(object target) {
            _target = new WeakReference(target);
        }

        public Action<object, EventArgs> Action { get; set; }
        public string PropertyName { get; set; }

        public bool IsAlive {
            get { return _source.IsAlive && _target.IsAlive; }
        }
    }
}
