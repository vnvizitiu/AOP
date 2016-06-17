using System;

namespace Aspects.Logging.Loggers
{
    /// <summary>
    ///  Represents a logger that logs to the console using color output
    /// </summary>
    /// <seealso cref="ILogger" />
    public sealed class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Logs the message with trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Logs the message with warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Logs the message and exception with error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine(exception);
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Logs the message and exception with fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(string message, Exception exception)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine(exception);
            Console.ForegroundColor = color;
        }
    }
}