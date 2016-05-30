using Aspects.Logging.Enums;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Exclude = ExclusionFlags.Constructors)]
    public class PersonExcludeConstructors
    {
        static PersonExcludeConstructors()
        {

        }

        public PersonExcludeConstructors()
        {
        }

        public string Name { get; set; }
    }
}