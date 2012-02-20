using System;

namespace Azon.Helpers.Events.Subscriptions.Infos {
    internal interface ISubscriptionInfo {
        Action<object, EventArgs> Action { get; }
        string PropertyName { get; set; }
        bool IsAlive { get; }
    }
}