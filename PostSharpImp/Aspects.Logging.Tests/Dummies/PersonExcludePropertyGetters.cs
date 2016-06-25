using Aspects.Logging;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Excludes = Excludes.PropertyGetters)]
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