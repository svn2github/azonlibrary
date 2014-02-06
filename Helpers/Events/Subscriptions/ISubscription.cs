using System;
using System.ComponentModel;

using Azon.Helpers.Annotations;

namespace Azon.Helpers.Events.Subscriptions {
    public interface ISubscription : IDisposable {
        [NotNull]
        IObjectSubscription<T> Object<T>(T entity) where T : INotifyPropertyChanged;
        void Unsubscribe();
    }
}