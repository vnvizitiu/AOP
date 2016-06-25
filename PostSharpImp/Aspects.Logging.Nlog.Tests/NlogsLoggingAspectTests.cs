namespace Aspects.Logging.Nlog.Tests
{
    using Aspects.Logging.Configuration.Abstract;
    using Aspects.Logging.Tests.Commons.Dummies;

    using FluentAssertions;

    using Moq;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    using NUnit.Framework;

    /// <summary>
    /// The nlogs logging aspect tests.
    /// </summary>
    [TestFixture]
    public class NlogsLoggingAspectTests
    {
        /// <summary>
        /// The using logging aspect with n log should use nlogger.
        /// </summary>
        [Test]
        public void UsingLoggingAspectWithNLogShouldUseNlogger()
        {
            // arrange - setup nlog
            LoggingConfiguration config = new LoggingConfiguration();

            MemoryTarget memoryTarget = new MemoryTarget { Layout = @"${message}" };
            config.AddTarget("memory", memoryTarget);

            LoggingRule rule = new LoggingRule("*", LogLevel.Debug, memoryTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            Mock<IConfigurationProvider> mock = new Mock<IConfigurationProvider>();
            mock.Setup(provider => provider.ShouldLog(It.IsAny<LogAttribute>())).Returns(true);
            LogAttribute.ConfigurationProvider = mock.Object;

            // arrange - setup logger
            LogAttribute.Logger = new NLogLogger("memory");

            // act
            Person person = new Person { Name = "test", Balance = 0.0d };
            person.Should().NotBeNull();

            // assert
            memoryTarget.Logs.Count.Should().Be(9, "because we called the logging 9 times");
        }
    }
}
