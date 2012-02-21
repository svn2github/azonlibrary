using System;

namespace Azon.Helpers.Events.Bindings {
    public class BindingException : Exception {
        public BindingException(string message) : base(message) {}

        public BindingException(string message, Exception inner) : base(message, inner) {}
    }
}