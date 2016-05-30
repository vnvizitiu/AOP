using Aspects.Logging.Enums;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Exclude = ExclusionFlags.PropertySetters)]
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