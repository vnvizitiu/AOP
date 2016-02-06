using System;

namespace LoggerAspect
{
    public class NullLogger : ILogger
    {
        public void Debug(string message) { }
        public void Error(string message, Exception exception) { }
    }
}