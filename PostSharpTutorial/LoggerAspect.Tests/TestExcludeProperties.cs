using System;
using FluentAssertions;
using LoggerAspect.Enums;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class TestExcludeProperties
    {
        private MockLogger _logger;

        [SetUp]
        public void InitializeLoggerAndAspect()
        {
            _logger = new MockLogger();
            LoggingAspect.Logger = _logger;
        }

        [Test]
        public void WhenAppliedToClassWithNoExclude_ShouldLogPropertyAndConstructor()
        {

            // act
            var p = new Person {Name = Guid.NewGuid().ToString()};

            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we hit the Entry, Success and Exit methods for both constructor and property");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropertyFlag_ShouldNotLogProperty()
        {
            // act
            var p = new PersonExcludeProperty {Name = Guid.NewGuid().ToString()};
            var a = p.Name;
            
            // assert
            _logger.DebugCallCount.Should()
                .Be(3, "because we only hit the Entry, Success and Exit methods for the constructor");  
        }

        [Test]
        public void WhenAppliedToClassWithExcludeInstanceConstructorFlag_ShouldNotLogInstanceConstructor()
        {
            // act
            var p = new PersonExcludeInstanceConstructor {Name = Guid.NewGuid().ToString()};
            var a = p.Name;
            
            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we only hit the Entry, Success and Exit methods for the property getter and setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludeStaticConstructorFlag_ShouldNotLogStaticConstructor()
        {
            // act
            var p = new PersonExcludeStaticConstructor {Name = Guid.NewGuid().ToString()};
            var a = p.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we only hit the Entry, Success and Exit methods for the instance constructor and property getter and setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropetieSettersFlag_ShouldNotLogPropertieSetters()
        {
            // act
            var p = new PersonExcludePropertySetters {Name = Guid.NewGuid().ToString()};
            var a = p.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we only hit the Entry, Success and Exit methods for the instance and static constructor and property getter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropertieGettersFlag_ShouldNotLogPropetieGetters()
        {
            // act
            var p = new PersonExcludePropertyGetters {Name = Guid.NewGuid().ToString()};
            var a = p.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we only hit the Entry, Success and Exit methods for the instance constructor and property setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludeConstructorsFlag_ShouldNotLogConstructors()
        {
            // act
            var p = new PersonExcludeConstructors {Name = Guid.NewGuid().ToString()};
            var a = p.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we only hit the Entry, Success and Exit methods for the property getter and setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropertiesAndConstructorsFlag_ShouldNotLogPropertiesAndConstructor()
        {
            // act
            var p = new PersonExcludePropertyConstructors {Name = Guid.NewGuid().ToString()};
            var a = p.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(0, "because we do not hit the Debug method for constructors and properties");
        }

    }
}
