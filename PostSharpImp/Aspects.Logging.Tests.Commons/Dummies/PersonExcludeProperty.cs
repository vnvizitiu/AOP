namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log(Excludes = Excludes.Properties)]
    public class PersonExcludeProperty
    {
        public string Name { get; set; }
    }
}