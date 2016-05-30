using Aspects.Logging.Enums;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Exclude = ExclusionFlags.Properties | ExclusionFlags.Constructors)]
    public class PersonExcludePropertyConstructors
    {
        static PersonExcludePropertyConstructors()
        {

        }

        public PersonExcludePropertyConstructors()
        {
        }

        public string Name { get; set; }
    }
}