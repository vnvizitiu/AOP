namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log(Excludes = Excludes.StaticConstructor)]
    public class PersonExcludeStaticConstructor
    {
        public string Name { get; set; }
    }
}