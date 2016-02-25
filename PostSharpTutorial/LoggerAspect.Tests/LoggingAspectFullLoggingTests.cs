using System;
using FluentAssertions;
using LoggerAspect.Tests.Dummies;
using LoggerAspect.Tests.Mocks;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class LoggingAspectFullLoggingTests
    {
        [Test]
        public void WhenTestingAFullTestcaseClass_ShouldLogEverything()
        {
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;

            var testClass = new FullTestClass();
            testClass.Value = Guid.NewGuid().ToString();
            var val = testClass.Value;

            testClass.EmbeddedMethods();

            logger.DebugCallCount.Should().Be(15, "because we call the Entry, Exit and Success methods for the constructor, property and methods, even private method");
        }
    }
}