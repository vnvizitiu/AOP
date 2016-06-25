namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The person exclude property constructors.
    /// </summary>
    [Log(Excludes = Excludes.Constructors | Excludes.Properties)]
    public class PersonExcludePropertyConstructors
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}