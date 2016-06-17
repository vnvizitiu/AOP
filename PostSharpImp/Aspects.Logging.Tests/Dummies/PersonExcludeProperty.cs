using Aspects.Logging;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Excludes = Excludes.Properties)]
    public class PersonExcludeProperty
    {
        public string Name { get; set; }
    }
}