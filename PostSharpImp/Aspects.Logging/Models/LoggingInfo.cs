namespace Aspects.Logging.Models
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// An option file used to confire the formatting.
    /// </summary>
    public class LoggingInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether log parameters.
        /// </summary>
        public bool LogParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether log return value.
        /// </summary>
        public bool LogReturnValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether log execution time.
        /// </summary>
        public bool LogExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the declaring type.
        /// </summary>
        public Type DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the method name.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets the stopwatch.
        /// </summary>
        public Stopwatch Stopwatch { get; set; }
    }
}