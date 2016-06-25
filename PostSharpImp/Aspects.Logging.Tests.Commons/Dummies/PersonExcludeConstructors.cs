namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The person exclude constructors.
    /// </summary>
    [Log(Excludes = Excludes.Constructors)]
    public class PersonExcludeConstructors
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}