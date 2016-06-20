namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log(Excludes = Excludes.PropertyGetters)]
    public class PersonExcludePropertyGetters
    {
        public string Name { get; set; }
    }
}