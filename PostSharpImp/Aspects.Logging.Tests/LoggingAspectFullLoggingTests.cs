using System;
using Aspects.Logging.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class LoggingAspectFullLoggingTests
    {
        [Test]
        public void WhenTestingAFullTestcaseClass_ShouldLogEverything()
        {
            var logger = new MockLogger();
            LogAttribute.Logger = logger;

            var testClass = new FullTestClass
            {
                Value = Guid.NewGuid().ToString()
            };
            var val = testClass.Value;

            testClass.EmbeddedMethods();

            logger.DebugCallCount.Should().Be(15, "because we call the Entry, Exit and Success methods for the constructor, property and methods, even private method");
        }

        [Test]
        public void WhenTestingAFullTestcaseClassWithDebugDisplay_ShouldLogEverythingAndTheObject()
        {
            var logger = new MockLogger();
            LogAttribute.Logger = logger;

            var testClass = CreateTestClass();
            var val = testClass.Value;

            testClass.EmbeddedMethods();

            logger.DebugCallCount.Should().Be(24, "because we call the Entry, Exit and Success methods for the constructor, property and methods, even private method");
        }

        [Log(LogReturnValue = true)]
        private static FullTestClass CreateTestClass()
        {
            return new FullTestClass
            {
                Value = Guid.NewGuid().ToString()
            };
        }
    }
}