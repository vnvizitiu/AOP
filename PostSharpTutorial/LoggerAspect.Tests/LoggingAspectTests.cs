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
            Assert.AreEqual(3, logger.DebugCallCount);
        }

        [Test]
        public void WhenCallingMethodWithException_ShouldHitDebugAndError()
        {
            try
            {
                var logger = new MockLogger();
                LoggingAspect.Logger = logger;
                ThrowsException();
                Assert.AreEqual(3, logger.DebugCallCount);
                Assert.AreEqual(1, logger.ErrorCallCount);
            }
            catch (Exception e)
            {
                Assert.IsAssignableFrom(typeof (NotImplementedException), e);
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
