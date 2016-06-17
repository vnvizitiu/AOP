using System;

namespace Aspects.Logging.Loggers
{
    /// <summary>
    ///  Represents a logger that does nothing 
    /// </summary>
    /// <seealso cref="ILogger" />
    public sealed class NullLogger : ILogger
    {
        /// <summary>
        /// Logs the message with trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message) { }

        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message) { }

        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message) { }

        /// <summary>
        /// Logs the message with warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message) { }

        /// <summary>
        /// Logs the message and exception with error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception) { }

        /// <summary>
        /// Logs the message and exception with fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(string message, Exception exception) { }
    }
}