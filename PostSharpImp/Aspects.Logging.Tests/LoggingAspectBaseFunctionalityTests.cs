using System;
using FluentAssertions;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class LoggingAspectBaseFunctionalityTests
    {
        private MockLogger _logger;

        [SetUp]
        public void InitializeTest()
        {
            _logger = new MockLogger();
            LogAttribute.Logger = _logger;
        }

        [Test]
        public void WhenCallingWorkingMethod_ShouldHitDebug3Times()
        {
            // act
            SomeMethod(1, null);

            // assert
            _logger.DebugCallCount.Should().Be(3, "because we only hit the Entry, Succes, and Exit methods");
        }

        [Test]
        public void WhenCallingMethodWithException_ShouldHitDebugAndError()
        {
            // act
            try
            {
                ThrowsException();
            }
            catch (Exception e)
                // we leave the base exception type here so we can catch any exception and later verify that it is the right type
            {
                // assert
                e.Should()
                    .BeOfType<NotImplementedException>("because we only explicitly threw a NotImplementedException");
                _logger.DebugCallCount.Should().Be(2, "becase we should only hit the Entry and Exit methods");
                _logger.ErrorCallCount.Should().Be(1, "because we should hit the Exception method");
            }
        }

        [Log(LogParameters = true)]
        public void SomeMethod(int num, int? num2)
        {
        }

        [Log]
        public void ThrowsException()
        {
            throw new NotImplementedException();
        }
    }
}