using System;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class LoggingAspectTests
    {
        [Test]
        public void WhenCallingWorkingMethod_ShouldHitDebug()
        {
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;
            SomeMethod();
            Assert.AreEqual(3, logger.DebugCallCount, "the Debug method was not called 3 time only (Entry, Succes, Error)");
        }

        [Test]
        public void WhenCallingMethodWithException_ShouldHitDebugAndError()
        {
            var logger = new MockLogger();
            LoggingAspect.Logger = logger;
            try
            {             
                ThrowsException();

            }
            catch (Exception e) // we leave the base exception type here so we can catch any exception and later verify that it is the right type
            {
                Assert.IsAssignableFrom(typeof (NotImplementedException), e, "the catched exception was not if type NotImplementedException");
                Assert.AreEqual(2, logger.DebugCallCount, "the Debug method was not called 2 times only (Entry, Exit)");
                Assert.AreEqual(1, logger.ErrorCallCount, "the Error method was not called only one (Exception)");
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
