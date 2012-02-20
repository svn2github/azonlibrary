using System;

namespace Azon.Helpers.Events.Bindings {
    [Flags]
    public enum BindingMode {
        OneWay = 1,
        OneWayToSource = 2,
        TwoWay = 3
    }
}