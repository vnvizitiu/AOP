using System;
using NUnit.Framework;
using Aspects.Logging.Interfaces;
using FluentAssertions;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class LoggerImplementationTests
    {
        private const string TestString = "TestString";

        [Test]
        public void WhenCallingDebug_CallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Debug(TestString);

            MockLogger mockLogger = (MockLogger) sut;
            mockLogger.DebugCallCount.Should().Be(1, "because we only called the debug method once");
        }

        [Test]
        public void WhenCallingInfo_CallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Info(TestString);

            MockLogger mockLogger = (MockLogger) sut;
            mockLogger.InfoCallCount.Should().Be(1, "because we only called the info method once");
        }

        [Test]
        public void WhenCallingTrace_CallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Trace(TestString);

            MockLogger mockLogger = (MockLogger) sut;
            mockLogger.TraceCallCount.Should().Be(1, "because we only called the trace method once");
        }

        [Test]
        public void WhenCallingError_CallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Error(TestString, new Exception());

            MockLogger mockLogger = (MockLogger) sut;
            mockLogger.ErrorCallCount.Should().Be(1, "because we only called the error method once");
        }

        [Test]
        public void WhenCallingFatal_CallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Fatal(TestString, new Exception());

            MockLogger mockLogger = (MockLogger) sut;
            mockLogger.FatalCallCount.Should().Be(1, "because we only called the fatal method once");
        }

        [Test]
        public void WhenCallingWarn_CallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Warn(TestString);

            MockLogger mockLogger = (MockLogger) sut;
            mockLogger.WarnCallCount.Should().Be(1, "because we only called the warn method once");
        }
    }
}
