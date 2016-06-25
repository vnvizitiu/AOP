namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The generic.
    /// </summary>
    /// <typeparam name="T"> a generic type
    /// </typeparam>
    [Log]
    public class Generic<T>
    {
        /// <summary>
        /// Gets or sets the my value.
        /// </summary>
        public T MyValue { get; set; }
    }
}