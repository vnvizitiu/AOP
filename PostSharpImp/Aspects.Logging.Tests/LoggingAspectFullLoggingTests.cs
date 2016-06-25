namespace Aspects.Logging.Tests
{
    using System;
    using Commons.Dummies;
    using FluentAssertions;
    using NUnit.Framework;
    using Utilities;

    [TestFixture]
    public class LoggingAspectFullLoggingTests
    {
        [Test]
        public void WhenTestingAFullTestcaseClassShouldLogEverything()
        {
            MockLogger logger = new MockLogger();
            LogAttribute.Logger = logger;

            FullTestClass testClass = new FullTestClass
            {
                Value = Guid.NewGuid().ToString()
            };
            testClass.Value.Should().NotBeNullOrEmpty();

            FullTestClass.EmbeddedMethod();

            logger.DebugCallCount.Should().Be(15, "because we call the Entry, Exit and Success methods for the constructor, property and methods, even private method");
        }

        [Test]
        public void WhenTestingAFullTestcaseClassWithDebugDisplayShouldLogEverythingAndTheObject()
        {
            MockLogger logger = new MockLogger();
            LogAttribute.Logger = logger;

            FullTestClass testClass = CreateTestClass();
            testClass.Value.Should().NotBeNullOrWhiteSpace();

            FullTestClass.EmbeddedMethod();

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