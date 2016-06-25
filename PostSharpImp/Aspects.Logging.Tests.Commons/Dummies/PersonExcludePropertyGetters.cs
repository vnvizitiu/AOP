namespace Aspects.Logging.Tests.Commons.Dummies
{
    /// <summary>
    /// The person exclude property getters.
    /// </summary>
    [Log(Excludes = Excludes.PropertyGetters)]
    public class PersonExcludePropertyGetters
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}