using Aspects.Logging;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Excludes = Excludes.PropertySetters)]
    public class PersonExcludePropertySetters
    {
        static PersonExcludePropertySetters()
        {

        }

        public PersonExcludePropertySetters()
        {
        }

        public string Name { get; set; }
    }
}