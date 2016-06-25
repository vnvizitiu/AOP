namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The person exclude static constructor.
    /// </summary>
    [Log(Excludes = Excludes.StaticConstructor)]
    public class PersonExcludeStaticConstructor
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}