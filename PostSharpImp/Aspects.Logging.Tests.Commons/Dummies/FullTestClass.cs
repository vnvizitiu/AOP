namespace Aspects.Logging.Tests.Commons.Dummies
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// The full test class.
    /// </summary>
    [Log(LogParameters = true, LogExecutionTime = true, LogReturnValue = true)]
    [DebuggerDisplay("{Value}")]
    public class FullTestClass
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The embedded method.
        /// </summary>
        public static void EmbeddedMethod()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            InnerMethod();
        }

        /// <summary>
        /// The inner method.
        /// </summary>
        private static void InnerMethod()
        {
            Thread.Sleep(TimeSpan.FromSeconds(.5));
        }
    }
}