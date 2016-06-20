namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log(Excludes = Excludes.InstanceConstructors)]
    public class PersonExcludeInstanceConstructor
    {
        public string Name { get; set; }
    }
}
