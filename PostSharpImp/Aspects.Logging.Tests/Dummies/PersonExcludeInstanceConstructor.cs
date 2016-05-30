using Aspects.Logging.Enums;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Exclude = ExclusionFlags.InstanceConstructors)]
    public class PersonExcludeInstanceConstructor
    {
        public PersonExcludeInstanceConstructor()
        {
        }

        public string Name { get; set; }
    }
}