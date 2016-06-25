namespace Aspects.Logging.Tests.Loggers
{
    using System;
    using Logging.Loggers;
    using NUnit.Framework;

    /// <summary>
    /// Tests present here just for code coverage
    /// </summary>
    [TestFixture]
    public class NullLoggerTests
    {
        /// <summary>
        /// The when calling debug should enter debug method.
        /// </summary>
        [Test]   
        public void WhenCallingDebugShouldEnterDebugMethod()
        {
            NullLogger logger = new NullLogger();

            logger.Debug("Test String");
        }

        /// <summary>
        /// The when calling trace should enter trace method.
        /// </summary>
        [Test]
        public void WhenCallingTraceShouldEnterTraceMethod()
        {
            NullLogger logger = new NullLogger();

            logger.Trace("Test String");
        }

        /// <summary>
        /// The when calling info should enter info method.
        /// </summary>
        [Test]
        public void WhenCallingInfoShouldEnterInfoMethod()
        {
            NullLogger logger = new NullLogger();

            logger.Info("Test String");
        }

        /// <summary>
        /// The when calling warn should enter warn method.
        /// </summary>
        [Test]
        public void WhenCallingWarnShouldEnterWarnMethod()
        {
            NullLogger logger = new NullLogger();

            logger.Warn("Test String");
        }

        /// <summary>
        /// The when calling error should enter error method.
        /// </summary>
        [Test]
        public void WhenCallingErrorShouldEnterErrorMethod()
        {
            NullLogger logger = new NullLogger();

            logger.Error("Test String", new Exception());
        }

        /// <summary>
        /// The when calling fatal should enter fatal method.
        /// </summary>
        [Test]
        public void WhenCallingFatalShouldEnterFatalMethod()
        {
            NullLogger logger = new NullLogger();

            logger.Fatal("Test String", new Exception());
        }
    }
}