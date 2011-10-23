using Azon.Helpers.Events.Subscriptions.Infos;

namespace Azon.Helpers.Events.Subscriptions.Subscribers {
    internal interface ISubscribeOn {
        void Attach(ISubscriptionInfo info);
        void Unsubscribe();
    }
}