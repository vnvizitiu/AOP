namespace Aspects.Logging.Tests.Loggers
{
    using System;

    using Aspects.Logging.Loggers;

    using NUnit.Framework;

    /// <summary>
    /// Tests present here just for code coverage
    /// </summary>
    [TestFixture]
    public class ConsoleLoggerTests
    {
        /// <summary>
        /// The when calling debug should enter debug method.
        /// </summary>
        [Test]
        public void WhenCallingDebugShouldEnterDebugMethod()
        {
            ConsoleLogger logger = new ConsoleLogger();

            logger.Debug("Test String");
        }

        /// <summary>
        /// The when calling trace should enter trace method.
        /// </summary>
        [Test]
        public void WhenCallingTraceShouldEnterTraceMethod()
        {
            ConsoleLogger logger = new ConsoleLogger();

            logger.Trace("Test String");
        }

        /// <summary>
        /// The when calling info should enter info method.
        /// </summary>
        [Test]
        public void WhenCallingInfoShouldEnterInfoMethod()
        {
            ConsoleLogger logger = new ConsoleLogger();

            logger.Info("Test String");
        }

        /// <summary>
        /// The when calling warn should enter warn method.
        /// </summary>
        [Test]
        public void WhenCallingWarnShouldEnterWarnMethod()
        {
            ConsoleLogger logger = new ConsoleLogger();

            logger.Warn("Test String");
        }

        /// <summary>
        /// The when calling error should enter error method.
        /// </summary>
        [Test]
        public void WhenCallingErrorShouldEnterErrorMethod()
        {
            ConsoleLogger logger = new ConsoleLogger();

            logger.Error("Test String", new Exception());
        }

        /// <summary>
        /// The when calling fatal should enter fatal method.
        /// </summary>
        [Test]
        public void WhenCallingFatalShouldEnterFatalMethod()
        {
            ConsoleLogger logger = new ConsoleLogger();

            logger.Fatal("Test String", new Exception());
        }
    }
}