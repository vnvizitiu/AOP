using System;

namespace ConsoleExample
{
    public class NullLogger : ILogger
    {
        public void Debug(string message) { }
        public void Error(string message, Exception exception) { }
    }
}