namespace Aspects.Logging.Nlog.Tests
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    using NUnit.Framework;

    /// <summary>
    /// The nlog logger wrapper tester.
    /// </summary>
    [TestFixture]
    public class NlogLoggerWrapperTests
    {
        /// <summary>
        /// The _logger.
        /// </summary>
        private NLogLogger _logger;

        /// <summary>
        /// The _memory target.
        /// </summary>
        private MemoryTarget _memoryTarget;

        /// <summary>
        /// The setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // arrange - setup nlog
            LoggingConfiguration config = new LoggingConfiguration();

            _memoryTarget = new MemoryTarget { Layout = @"${level} ${message}" };
            config.AddTarget("memory", _memoryTarget);

            LoggingRule rule = new LoggingRule("*", LogLevel.Trace, _memoryTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            // arrange - setup logger
            _logger = new NLogLogger("memory");
        }

        /// <summary>
        /// The cleanup.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _logger = null;
            _memoryTarget.Dispose();
        }

        /// <summary>
        /// The when calling error should record one error.
        /// </summary>
        [Test]
        public void WhenCallingErrorShouldRecordOneError()
        {
            // act
            _logger.Error("Test String", new NotImplementedException());

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Error")).Should().BeTrue("Because we only logged an Error");
        }

        /// <summary>
        /// The when calling fatal should record one error.
        /// </summary>
        [Test]
        public void WhenCallingFatalShouldRecordOneError()
        {
            // act
            _logger.Fatal("Test String", new NotImplementedException());

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Fatal")).Should().BeTrue("Because we only logged a Fatal");
        }

        /// <summary>
        /// The when calling info should record one error.
        /// </summary>
        [Test]
        public void WhenCallingInfoShouldRecordOneError()
        {
            // act
            _logger.Info("Test String");

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Info")).Should().BeTrue("Because we only logged a Info");
        }

        /// <summary>
        /// The when calling trace should record one error.
        /// </summary>
        [Test]
        public void WhenCallingTraceShouldRecordOneError()
        {
            // act
            _logger.Trace("Test String");

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Trace")).Should().BeTrue("Because we only logged an Trace");
        }

        /// <summary>
        /// The when calling warn should record one error.
        /// </summary>
        [Test]
        public void WhenCallingWarnShouldRecordOneError()
        {
            // act
            _logger.Warn("Test String");

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Warn")).Should().BeTrue("Because we only logged an Warn");
        }
    }
}