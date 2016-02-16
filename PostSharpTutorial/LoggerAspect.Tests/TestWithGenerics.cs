using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
            var logger = new MockLogger();;
            LoggingAspect.Logger = logger;
            var g = new Generic<int>();
            g.MyValue = 100;

            logger.DebugCallCount.Should().Be(6) ;
        }

        [Test]
        public void TestTwoGeneric()
        {

            var logger = new MockLogger();
            LoggingAspect.Logger = logger;
            var gInt = new Generic<int>();
            gInt.MyValue = 100;

            var gStr = new Generic<string>();
            gStr.MyValue = "andrei";
            logger.DebugCallCount.Should().Be(12);
        }
    }
}
