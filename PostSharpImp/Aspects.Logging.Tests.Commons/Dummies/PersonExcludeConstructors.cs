namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log(Excludes = Excludes.Constructors)]
    public class PersonExcludeConstructors
    {
        public string Name { get; set; }
    }
}