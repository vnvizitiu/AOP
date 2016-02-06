using System;
using Moq;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class LoggingAspectTests
    {
        [Test]
        public void WhenCallingWorkingMethod_ShouldHitDebug()
        {
            var logger = new Mock<ILogger>();
            LoggingAspect.Logger = logger.Object;
            SomeMethod();
            logger.Verify(logger1 => logger1.Debug(It.IsAny<string>()));
        }

        [Test]
        public void WhenCallingMethodWithException_ShouldHitDebugAndError()
        {
            try
            {
                var logger = new Mock<ILogger>();
                LoggingAspect.Logger = logger.Object;
                ThrowsException();
                logger.Verify(logger1 => logger1.Debug(It.IsAny<string>()));
                logger.Verify(logger1 => logger1.Error(It.IsAny<string>(), It.IsAny<Exception>()));
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
