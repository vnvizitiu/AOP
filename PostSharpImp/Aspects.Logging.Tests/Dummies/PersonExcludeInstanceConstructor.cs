using Aspects.Logging;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Excludes = Excludes.InstanceConstructors)]
    public class PersonExcludeInstanceConstructor
    {
        public PersonExcludeInstanceConstructor()
        {
        }

        public string Name { get; set; }
    }
}