using Aspects.Logging;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Excludes = Excludes.Properties | Excludes.Constructors)]
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