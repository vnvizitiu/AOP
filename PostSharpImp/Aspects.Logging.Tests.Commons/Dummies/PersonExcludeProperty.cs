namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The person exclude property.
    /// </summary>
    [Log(Excludes = Excludes.Properties)]
    public class PersonExcludeProperty
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}