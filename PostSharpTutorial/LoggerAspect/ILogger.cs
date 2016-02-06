using System;

namespace LoggerAspect
{
    /// <summary>
    /// Interface for implementing a logger
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);

        /// <summary>
        /// Logs the message and exception with error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Logs the message and exception with fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        /// <summary>
        /// Logs the message with trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Trace(string message);

        /// <summary>
        /// Logs the message with warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(string message);
    }
}