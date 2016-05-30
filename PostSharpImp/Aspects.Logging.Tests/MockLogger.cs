using System;
using Aspects.Logging.Interfaces;

namespace Aspects.Logging.Tests
{
    public class MockLogger : ILogger
    {
        public int DebugCallCount { get; private set; }
        public int InfoCallCount { get; private set; }
        public int TraceCallCount { get; private set; }
        public int ErrorCallCount { get; private set; }
        public int FatalCallCount { get; private set; }
        public int WarnCallCount { get; private set; }

        public void Debug(string message)
        {
            Console.WriteLine(message);
            DebugCallCount++;
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
            InfoCallCount++;
        }

        public void Trace(string message)
        {
            Console.WriteLine(message);
            TraceCallCount++;
        }

        public void Fatal(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
            FatalCallCount++;
        }

        public void Error(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
            ErrorCallCount++;
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
            WarnCallCount++;
        }
    }
}