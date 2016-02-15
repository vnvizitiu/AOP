using System;
using NLog;
using ILogger = LoggerAspect.Interfaces.ILogger;

namespace LoggerAspect.Nlog
{
    /// <summary>
    /// A logger that implements Nlog
    /// </summary>
    public class NLogLogger : ILogger
    {
        private readonly Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogger"/> class.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        public NLogLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        /// <summary>
        /// Logs the message and exception with error level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        /// <summary>
        /// Logs the message and exception with fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }

        /// <summary>
        /// Logs the message with debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        /// Logs the message with trace level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        /// <summary>
        /// Logs the message with warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}