using System;
using FluentAssertions;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class LoggingAspectBaseFunctionalityTests
    {
        [Test]
        public void WhenCallingWorkingMethod_ShouldHitDebug3Times()
        {
            // arrange
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;
            // act
            SomeMethod();

            // assert
            logger.DebugCallCount.Should().Be(3, "because we only hit the Entry, Succes, and Exit methods");
        }

        [Test]
        public void WhenCallingMethodWithException_ShouldHitDebugAndError()
        {
            // arrange
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;

            // act
            try
            {             
                ThrowsException();
            }
            catch (Exception e) // we leave the base exception type here so we can catch any exception and later verify that it is the right type
            {
                // assert
                e.Should().BeOfType<NotImplementedException>("because we only explictly threw a NotImplementedException");
                logger.DebugCallCount.Should().Be(2, "becase we should only hit the Entry and Exit methods");
                logger.ErrorCallCount.Should().Be(1, "because we should hit the Exception method");
            }
        }

        [LoggingAspect]
        public void SomeMethod()
        {
        }

        [LoggingAspect]
        public void ThrowsException()
        {
            throw new NotImplementedException();
        }
    }
}
