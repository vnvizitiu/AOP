using Aspects.Logging.Enums;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Exclude = ExclusionFlags.StaticConstructor)]
    public class PersonExcludeStaticConstructor
    {
        static PersonExcludeStaticConstructor()
        {

        }


        public string Name { get; set; }
    }
}