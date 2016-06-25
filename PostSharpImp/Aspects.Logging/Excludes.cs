namespace Aspects.Logging
{
    using System;

    /// <summary>
    /// The exclude options.
    /// </summary>
    [Flags]
    public enum Excludes
    {
        /// <summary>
        /// The none.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        None = 0, 

        /// <summary>
        /// The static constructor.
        /// </summary>
        StaticConstructor = 1 << 0, 

        /// <summary>
        /// The instance constructors.
        /// </summary>
        InstanceConstructors = 1 << 1, 

        /// <summary>
        /// The property getters.
        /// </summary>
        PropertyGetters = 1 << 2, 

        /// <summary>
        /// The property setters.
        /// </summary>
        PropertySetters = 1 << 3, 

        /// <summary>
        /// The properties.
        /// </summary>
        Properties = PropertyGetters | PropertySetters, 

        /// <summary>
        /// The constructors.
        /// </summary>
        Constructors = StaticConstructor | InstanceConstructors
    }
}