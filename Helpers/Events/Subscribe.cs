using Azon.Helpers.Events.Subscriptions;

namespace Azon.Helpers.Events {
    public class Subscribe {
        public static ISubscription When {
            get { return new AggregateSubscription(); }
        }
    }
}
