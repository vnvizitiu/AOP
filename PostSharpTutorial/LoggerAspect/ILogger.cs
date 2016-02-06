using System;

namespace LoggerAspect
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(string message, Exception exception);
    }
}