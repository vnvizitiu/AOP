using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [LoggingAspect]
    public class Generic<T>
    {
        public T MyValue { get; set; }
    }

    [TestFixture]
    public class TestWithGenerics
    {
        [Test]
        public void TestOneGeneric()
        {
            var logger = new Mock<ILogger>();
            LoggingAspect.Logger = logger.Object;
            var g = new Generic<int>();
            g.MyValue = 100;

            logger.Verify(logger1 => logger1.Debug(It.IsAny<string>()), Times.Exactly(6));
        }

        [Test]
        public void TestTwoGeneric()
        {

            var logger = new Mock<ILogger>();
            LoggingAspect.Logger = logger.Object;
            var gInt = new Generic<int>();
            gInt.MyValue = 100;

            var gStr = new Generic<string>();
            gStr.MyValue = "andrei";
            logger.Verify(logger1 => logger1.Debug(It.IsAny<string>()), Times.Exactly(12));
        }
    }
}
