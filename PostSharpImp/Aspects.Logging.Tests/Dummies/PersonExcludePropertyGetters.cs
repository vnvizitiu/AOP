using Aspects.Logging.Enums;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Exclude = ExclusionFlags.PropertyGetters)]
    public class PersonExcludePropertyGetters
    {
        static PersonExcludePropertyGetters()
        {

        }

        public PersonExcludePropertyGetters()
        {
        }

        public string Name { get; set; }
    }
}