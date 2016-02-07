using System;

namespace LoggerAspect
{
    public class NullLogger : ILogger
    {
        public void Debug(string message) { }
        public void Error(string message, Exception exception) { }
        public void Fatal(string message, Exception exception) { }

        public void Info(string message) { }

        public void Trace(string message) { }

        public void Warn(string message) { }
    }
}