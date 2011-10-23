namespace Azon.Helpers.Events.Subscriptions.Infos {
    internal class ChangedSubscriptionInfo : SubscriptionInfo {
        public ChangedSubscriptionInfo(object entity, string propertyName) : base(entity, propertyName) {}
    }
}