using Aspects.Logging.Enums;

namespace Aspects.Logging.Tests.Dummies
{
    [Log(Exclude = ExclusionFlags.Properties)]
    public class PersonExcludeProperty
    {
        public string Name { get; set; }
    }
}