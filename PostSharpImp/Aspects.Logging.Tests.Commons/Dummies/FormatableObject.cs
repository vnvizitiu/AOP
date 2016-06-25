namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The formatable object.
    /// </summary>
    public class FormatableObject
    {
        /// <summary>
        /// The int field.
        /// </summary>
        // ReSharper disable StyleCop.SA1401
        // ReSharper disable once NotAccessedField.Global
        public int IntField;
        // ReSharper restore StyleCop.SA1401

        /// <summary>
        /// Gets or sets the string property.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string StringProperty { get; set; }

        /// <summary>
        /// Gets or sets the double property.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public double DoubleProperty { get; set; }
    }
}