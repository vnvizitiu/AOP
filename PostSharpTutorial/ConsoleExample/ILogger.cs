using System;

namespace ConsoleExample
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(string message, Exception exception);
    }
}