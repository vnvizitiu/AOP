using System;
using System.Linq;
using FluentAssertions;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace Aspects.Logging.Nlog.Tests
{
    [TestFixture]
    public class NlogLoggerWrapperTester
    {
        private NLogLogger _logger;
        private MemoryTarget _memoryTarget;

        [SetUp]
        public void Setup()
        {
            // arrange - setup nlog
            LoggingConfiguration config = new LoggingConfiguration();

            _memoryTarget = new MemoryTarget();
            _memoryTarget.Layout = @"${level} ${message}";
            config.AddTarget("memory", _memoryTarget);

            LoggingRule rule = new LoggingRule("*", LogLevel.Trace, _memoryTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            // arrange - setup logger
            _logger = new NLogLogger("memory");
        }

        [TearDown]
        public void Cleanup()
        {
            _logger = null;
            _memoryTarget.Dispose();
        }

        [Test]
        public void WhenCallingError_ShouldRecordOneError()
        {
            // act
            _logger.Error("Test String", new NotImplementedException());

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Error")).Should().BeTrue("Because we only logged an Error");
        }

        [Test]
        public void WhenCallingFatal_ShouldRecordOneError()
        {
            // act
            _logger.Fatal("Test String", new NotImplementedException());

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Fatal")).Should().BeTrue("Because we only logged a Fatal");
        }

        [Test]
        public void WhenCallingInfo_ShouldRecordOneError()
        {
            // act
            _logger.Info("Test String");

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Info")).Should().BeTrue("Because we only logged a Info");
        }

        [Test]
        public void WhenCallingTrace_ShouldRecordOneError()
        {
            // act
            _logger.Trace("Test String");

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Trace")).Should().BeTrue("Because we only logged an Trace");
        }

        [Test]
        public void WhenCallingWarn_ShouldRecordOneError()
        {
            // act
            _logger.Warn("Test String");

            // assert
            _memoryTarget.Logs.Count.Should().Be(1, "because we only called the method once");
            _memoryTarget.Logs.All(log => log.Contains("Warn")).Should().BeTrue("Because we only logged an Warn");
        }
    }
}