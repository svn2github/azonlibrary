using System;

namespace Azon.Helpers.Events.Bindings {
    internal enum ErrorMode {
        Throw,
        Skip,
        Notify
    }

    internal class BindindErrorOptions {
        public ErrorMode Mode { get; set; }
        public Action<BindingException> Callback { get; set; }
    }
}