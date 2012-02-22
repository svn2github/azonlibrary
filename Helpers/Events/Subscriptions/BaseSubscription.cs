using System.ComponentModel;

using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Subscriptions {
    internal abstract class BaseSubscription : ISubscription {
        public abstract IObjectSubscription<T> Object<T>(T entity) where T : INotifyPropertyChanged;
        protected abstract void UnsubscribeCore();

        public void Unsubscribe() {
            Guard.Block(this, this.UnsubscribeCore);
        }

        public void Dispose() {
            this.Unsubscribe();
        }
    }
}