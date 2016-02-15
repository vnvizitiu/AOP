using System;

namespace LoggerAspect
{
    [Flags]
    public enum ExclusionFlags
    {
        None = 1 << 0,
        StaticConstructor = 1 << 1,
        InstanceConstructors = 1 << 2,
        PropertyGetters = 1 << 3,
        PropertySetters = 1 << 4,
        Properties = PropertyGetters | PropertySetters,
        Constructors = StaticConstructor | InstanceConstructors
    }
}