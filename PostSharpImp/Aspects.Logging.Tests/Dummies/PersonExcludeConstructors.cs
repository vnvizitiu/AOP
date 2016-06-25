using Aspects.Logging;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Excludes = Excludes.Constructors)]
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