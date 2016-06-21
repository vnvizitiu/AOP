using System;
using Aspects.Logging.Loggers;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class ConsoleLoggerTests
    {
        [Test]
        public void WhenCallingDebug_ShouldEnterDebugMethod()
        {
            var logger = new ConsoleLogger();

            logger.Debug("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingTrace_ShouldEnterTraceMethod()
        {
            var logger = new ConsoleLogger();

            logger.Trace("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingInfo_ShouldEnterInfoMethod()
        {
            var logger = new ConsoleLogger();

            logger.Info("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingWarn_ShouldEnterWarnMethod()
        {
            var logger = new ConsoleLogger();

            logger.Warn("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingError_ShouldEnterErrorMethod()
        {
            var logger = new ConsoleLogger();

            logger.Error("Test String", new Exception());

            Assert.Pass();
        }

        [Test]
        public void WhenCallingFatal_ShouldEnterFatalMethod()
        {
            var logger = new ConsoleLogger();

            logger.Fatal("Test String", new Exception());

            Assert.Pass();
        }
    }
}