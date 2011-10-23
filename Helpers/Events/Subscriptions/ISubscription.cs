using System;
using System.ComponentModel;

namespace Azon.Helpers.Events.Subscriptions {
    public interface ISubscription : IDisposable {
        IObjectSubscription<T> Object<T>(T entity) where T : INotifyPropertyChanged;
        void Unsubscribe();
    }
}