namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The person exclude instance constructor.
    /// </summary>
    [Log(Excludes = Excludes.InstanceConstructors)]
    public class PersonExcludeInstanceConstructor
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
