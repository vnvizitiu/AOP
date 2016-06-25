namespace Aspects.Logging.Tests
{
    using System;
    using FluentAssertions;
    using Logging.Loggers;
    using NUnit.Framework;
    using Utilities;

    [TestFixture]
    public class LoggerImplementationTests
    {
        private const string TestString = "TestString";

        /// <summary>
        /// The when calling debug call count should increase.
        /// </summary>
        [Test]
        public void WhenCallingDebugCallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Debug(TestString);

            MockLogger mockLogger = (MockLogger)sut;
            mockLogger.DebugCallCount.Should().Be(1, "because we only called the debug method once");
        }

        /// <summary>
        /// The when calling info call count should increase.
        /// </summary>
        [Test]
        public void WhenCallingInfoCallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Info(TestString);

            MockLogger mockLogger = (MockLogger)sut;
            mockLogger.InfoCallCount.Should().Be(1, "because we only called the info method once");
        }

        /// <summary>
        /// The when calling trace call count should increase.
        /// </summary>
        [Test]
        public void WhenCallingTraceCallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Trace(TestString);

            MockLogger mockLogger = (MockLogger)sut;
            mockLogger.TraceCallCount.Should().Be(1, "because we only called the trace method once");
        }

        /// <summary>
        /// The when calling error call count should increase.
        /// </summary>
        [Test]
        public void WhenCallingErrorCallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Error(TestString, new Exception());

            MockLogger mockLogger = (MockLogger)sut;
            mockLogger.ErrorCallCount.Should().Be(1, "because we only called the error method once");
        }

        /// <summary>
        /// The when calling fatal call count should increase.
        /// </summary>
        [Test]
        public void WhenCallingFatalCallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Fatal(TestString, new Exception());

            MockLogger mockLogger = (MockLogger)sut;
            mockLogger.FatalCallCount.Should().Be(1, "because we only called the fatal method once");
        }

        /// <summary>
        /// The when calling warn call count should increase.
        /// </summary>
        [Test]
        public void WhenCallingWarnCallCountShouldIncrease()
        {
            ILogger sut = new MockLogger();
            sut.Warn(TestString);

            MockLogger mockLogger = (MockLogger)sut;
            mockLogger.WarnCallCount.Should().Be(1, "because we only called the warn method once");
        }
    }
}
