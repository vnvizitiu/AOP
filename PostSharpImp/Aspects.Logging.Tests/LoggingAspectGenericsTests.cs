using Aspects.Logging.Tests.Commons.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class LoggingAspectGenericsTests
    {
        private MockLogger _logger;

        [SetUp]
        public void InitializeTest()
        {
            _logger = new MockLogger();
            LogAttribute.Logger = _logger;
        }

        [Test]
        public void TestOneGeneric()
        {
            var generic = new Generic<int>
            {
                MyValue = 100
            };
            var value = generic.MyValue;
            _logger.DebugCallCount.Should().Be(9, "because we expect it to enter the Entry, Exit and Success methods for both constructor and property");
        }

        [Test]
        public void TestTwoGeneric()
        {
            var gInt = new Generic<int>
            {
                MyValue = 100
            };
            var valueInt = gInt.MyValue;
            var gStr = new Generic<string>
            {
                MyValue = "andrei"
            };
            var valueStr = gStr.MyValue;
            _logger.DebugCallCount.Should().Be(18, "because we expect it to enter the Entry, Exit and Success for both constructor and property on both instances");
        }
    }
}