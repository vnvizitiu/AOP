namespace Aspects.Logging.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using Utilities;

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

        /// <summary>
        /// The when calling working method should hit debug 3 times.
        /// </summary>
        [Test]
        public void WhenCallingWorkingMethodShouldHitDebug3Times()
        {
            // act
            SomeMethod(1, null);

            // assert
            _logger.DebugCallCount.Should().Be(3, "because we only hit the Entry, Success, and Exit methods");
        }

        /// <summary>
        /// The when calling method with exception should hit debug and error.
        /// </summary>
        [Test]
        public void WhenCallingMethodWithExceptionShouldHitDebugAndError()
        {
            // act
            try
            {
                ThrowsException();
            }
            catch (Exception e)
            {
                // we leave the base exception type here so we can catch any exception and later verify that it is the right type
                // assert
                e.Should()
                    .BeOfType<NotImplementedException>("because we only explicitly threw a NotImplementedException");
                _logger.DebugCallCount.Should().Be(2, "becase we should only hit the Entry and Exit methods");
                _logger.ErrorCallCount.Should().Be(1, "because we should hit the Exception method");
            }
        }

        [Log(LogParameters = true)]
        // ReSharper disable UnusedParameter.Local
        private static void SomeMethod(int num, int? num2)
        // ReSharper restore UnusedParameter.Local
        {
        }

        [Log]
        private static void ThrowsException()
        {
            throw new NotImplementedException();
        }
    }
}