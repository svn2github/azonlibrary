using Azon.Helpers.Constructs.SwitchType;

namespace Azon.Helpers.Constructs {
    public static class Switch {
        public static ISwitchType<T> Type<T>(T value, bool exactType = false) {
            return new SwitchType<T>(exactType, value);
        }

        public static ISwitchType<T> Type<T>(bool exactType = false) {
            return new SwitchType<T>(exactType, default(T));
        }

        public static ISwitchTypeWithResult<T, TResult> Type<T, TResult>(T value, bool exactType = false) {
            return new SwitchTypeWithResult<T, TResult>(exactType, value);
        }

        public static ISwitchTypeWithResult<T, TResult> Type<T, TResult>(bool exactType = false) {
            return new SwitchTypeWithResult<T, TResult>(exactType, default(T));
        }
    }
}
