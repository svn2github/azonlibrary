using System;

using Azon.Helpers.Utils;

namespace Azon.Helpers.Events.Subscriptions.Infos {
    internal interface ISubscriptionInfo {
        Weak<Action<object, EventArgs>> Action { get; }
        string PropertyName { get; set; }
    }
}