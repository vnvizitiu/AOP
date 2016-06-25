namespace Aspects.Logging.Tests.Utilities
{
    using System;

    using Aspects.Logging.Loggers;

    /// <summary>
    /// The mock logger.
    /// </summary>
    public class MockLogger : ILogger
    {
        /// <summary>
        /// Gets the debug call count.
        /// </summary>
        public int DebugCallCount { get; private set; }

        /// <summary>
        /// Gets the info call count.
        /// </summary>
        public int InfoCallCount { get; private set; }

        /// <summary>
        /// Gets the trace call count.
        /// </summary>
        public int TraceCallCount { get; private set; }

        /// <summary>
        /// Gets the error call count.
        /// </summary>
        public int ErrorCallCount { get; private set; }

        /// <summary>
        /// Gets the fatal call count.
        /// </summary>
        public int FatalCallCount { get; private set; }

        /// <summary>
        /// Gets the warn call count.
        /// </summary>
        public int WarnCallCount { get; private set; }

        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Debug(string message)
        {
            Console.WriteLine(message);
            DebugCallCount++;
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Info(string message)
        {
            Console.WriteLine(message);
            InfoCallCount++;
        }

        /// <summary>
        /// The trace.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Trace(string message)
        {
            Console.WriteLine(message);
            TraceCallCount++;
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Fatal(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
            FatalCallCount++;
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void Error(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
            ErrorCallCount++;
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Warn(string message)
        {
            Console.WriteLine(message);
            WarnCallCount++;
        }
    }
}