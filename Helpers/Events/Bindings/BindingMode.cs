using System;

namespace Azon.Helpers.Events.Bindings {
    [Flags]
    internal enum BindingMode {
        OneWay = 1,
        OneWayToSource = 2,
        TwoWay = 3
    }
}