using System;

namespace LoggerAspect.Tests
{
    public class MockLogger : ILogger
    {
        public int DebugCallCount { get; set; }
        public int ErrorCallCount { get; set; }
        public int FatalCallCount { get; set; }
        public int InfoCallCount { get; set; }
        public int TraceCallCount { get; set; }
        public int WarnCallCount { get; set; }


        public void Debug(string message)
        {
            Console.WriteLine(message);
            DebugCallCount++;
        }

        public void Error(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
            ErrorCallCount++;
        }

        public void Fatal(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
            FatalCallCount++;
        }

        public void Info(string message)
        {
            Console.WriteLine(message); InfoCallCount++;
        }

        public void Trace(string message)
        {
            Console.WriteLine(message); TraceCallCount++;
        }

        public void Warn(string message)
        {
            Console.WriteLine(message); WarnCallCount++;
        }
    }
}