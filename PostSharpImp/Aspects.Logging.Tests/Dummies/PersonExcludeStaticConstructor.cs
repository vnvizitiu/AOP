using Aspects.Logging;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Excludes = Excludes.StaticConstructor)]
    public class PersonExcludeStaticConstructor
    {
        static PersonExcludeStaticConstructor()
        {

        }


        public string Name { get; set; }
    }
}