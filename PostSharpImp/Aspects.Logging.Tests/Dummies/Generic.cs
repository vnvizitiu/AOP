namespace Aspects.Logging.Tests.Dummies
{
    [Log]
    public class Generic<T>
    {
        public T MyValue { get; set; }
    }
}