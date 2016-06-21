using System;
using Aspects.Logging.Loggers;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class NullLoggerTests
    {
        [Test]
        public void WhenCallingDebug_ShouldEnterDebugMethod()
        {
            var logger = new NullLogger();

            logger.Debug("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingTrace_ShouldEnterTraceMethod()
        {
            var logger = new NullLogger();

            logger.Trace("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingInfo_ShouldEnterInfoMethod()
        {
            var logger = new NullLogger();

            logger.Info("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingWarn_ShouldEnterWarnMethod()
        {
            var logger = new NullLogger();

            logger.Warn("Test String");

            Assert.Pass();
        }

        [Test]
        public void WhenCallingError_ShouldEnterErrorMethod()
        {
            var logger = new NullLogger();

            logger.Error("Test String", new Exception());

            Assert.Pass();
        }

        [Test]
        public void WhenCallingFatal_ShouldEnterFatalMethod()
        {
            var logger = new NullLogger();

            logger.Fatal("Test String", new Exception());

            Assert.Pass();
        }
    }
}