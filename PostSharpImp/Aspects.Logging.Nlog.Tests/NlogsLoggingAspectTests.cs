using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspects.Logging.Tests.Commons.Dummies;
using FluentAssertions;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Aspects.Logging.Nlog.Tests
{
    [TestFixture]
    public class NlogsLoggingAspectTests
    {
        [Test]
        public void UsingLoggingAspectWithNLog_ShouldUseNlogger()
        {
            // arrange - setup nlog
            LoggingConfiguration config = new LoggingConfiguration();

            MemoryTarget memoryTarget = new MemoryTarget();
            memoryTarget.Layout = @"${message}";
            config.AddTarget("memory", memoryTarget);

            LoggingRule rule = new LoggingRule("*", LogLevel.Debug, memoryTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            // arrange - setup logger
            LogAttribute.Logger = new NLogLogger("memory");

            // act
            Person person = new Person()
            {
                Name = "test",
                Balance = 0.0d
            };

            // assert
            memoryTarget.Logs.Count.Should().Be(9, "because we called the logging 6 times");
        }
    }
}
