namespace Aspects.Logging.Tests.Commons.Dummies
{
    [Log]
    public class Generic<T>
    {
        public T MyValue { get; set; }
    }
}