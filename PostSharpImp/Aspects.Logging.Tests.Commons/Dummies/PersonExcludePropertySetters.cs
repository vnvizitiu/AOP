namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log(Excludes = Excludes.PropertySetters)]
    public class PersonExcludePropertySetters
    {
        static PersonExcludePropertySetters()
        {
        }

        public string Name { get; set; }
    }
}