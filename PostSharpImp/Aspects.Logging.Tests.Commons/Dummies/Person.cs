namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The person.
    /// </summary>
    [Log]
    public class Person
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public double Balance { get; set; }
    }
}