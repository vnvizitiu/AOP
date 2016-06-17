using System;

namespace Aspects.Logging
{
    [Flags]
    public enum Excludes
    {
        None = 0,
        StaticConstructor = 1 << 0,
        InstanceConstructors = 1 << 1,
        PropertyGetters = 1 << 2,
        PropertySetters = 1 << 3,
        Properties = PropertyGetters | PropertySetters,
        Constructors = StaticConstructor | InstanceConstructors
    }
}