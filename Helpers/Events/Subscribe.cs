using Azon.Helpers.Events.Subscriptions;

namespace Azon.Helpers.Events {
    /// <summary>
    /// Helper class to simplify event subscription.
    /// </summary>
    public class Subscribe {
        public static ISubscription When {
            get { return new AggregateSubscription(); }
        }
    }
}
