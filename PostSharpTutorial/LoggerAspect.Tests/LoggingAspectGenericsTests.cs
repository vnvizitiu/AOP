using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LoggerAspect.Tests.Dummies;
using LoggerAspect.Tests.Mocks;
using NUnit.Framework;

namespace LoggerAspect.Tests
{

    [TestFixture]
    public class LoggingAspectGenericsTests
    {
        private MockLogger _logger;

        [SetUp]
        public void InitializeTest()
        {
            _logger = new MockLogger();
            LoggingAspect.Logger = _logger;
        }

        [Test]
        public void TestOneGeneric()
        {
            var g = new Generic<int>();
            g.MyValue = 100;

            _logger.DebugCallCount.Should().Be(6, "because we expect it to enter the Entry, Exit and Success methods for both constructor and property") ;
        }

        [Test]
        public void TestTwoGeneric()
        {
            var gInt = new Generic<int>();
            gInt.MyValue = 100;

            var gStr = new Generic<string>();
            gStr.MyValue = "andrei";
            _logger.DebugCallCount.Should().Be(12, "because we expect it to enter the Entry, Exit and Success for both constructor and property on both instances");
        }
    }
}
