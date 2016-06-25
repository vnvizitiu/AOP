namespace Aspects.Logging.Tests
{
    using Commons.Dummies;
    using FluentAssertions;
    using NUnit.Framework;
    using Utilities;

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
            Generic<int> generic = new Generic<int>
            {
                MyValue = 100
            };
            generic.MyValue.Should().Be(100);
            _logger.DebugCallCount.Should().Be(9, "because we expect it to enter the Entry, Exit and Success methods for both constructor and property");
        }

        [Test]
        public void TestTwoGeneric()
        {
            Generic<int> int1 = new Generic<int>
            {
                MyValue = 100
            };
            int1.MyValue.Should().Be(100);
            Generic<string> str = new Generic<string>
            {
                MyValue = "andrei"
            };
            str.MyValue.Should().NotBeNullOrWhiteSpace();
            _logger.DebugCallCount.Should().Be(18, "because we expect it to enter the Entry, Exit and Success for both constructor and property on both instances");
        }
    }
}