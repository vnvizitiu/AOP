using System;
using Aspects.Logging.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class ExcludeTests
    {
        private MockLogger _logger;

        [SetUp]
        public void InitializeLoggerAndAspect()
        {
            _logger = new MockLogger();
            LogAttribute.Logger = _logger;
        }

        [Test]
        public void WhenAppliedToClassWithNoExclude_ShouldLogPropertyAndConstructor()
        {
            // act
            var person = new Person
            {
                Name = Guid.NewGuid().ToString()
            };
            var personName = person.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we hit the Entry, Success and Exit methods for both constructor and property");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropertyFlag_ShouldNotLogProperty()
        {
            // act
            var personExcludeProperty = new PersonExcludeProperty
            {
                Name = Guid.NewGuid().ToString()
            };
            var name = personExcludeProperty.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(3, "because we only hit the Entry, Success and Exit methods for the constructor");
        }

        [Test]
        public void WhenAppliedToClassWithExcludeInstanceConstructorFlag_ShouldNotLogInstanceConstructor()
        {
            // act
            var personExcludeInstanceConstructor = new PersonExcludeInstanceConstructor
            {
                Name = Guid.NewGuid().ToString()
            };
            var name = personExcludeInstanceConstructor.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we only hit the Entry, Success and Exit methods for the property getter and setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludeStaticConstructorFlag_ShouldNotLogStaticConstructor()
        {
            // act
            var personExcludeStaticConstructor = new PersonExcludeStaticConstructor
            {
                Name = Guid.NewGuid().ToString()
            };
            var name = personExcludeStaticConstructor.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we only hit the Entry, Success and Exit methods for the instance constructor and property getter and setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropetieSettersFlag_ShouldNotLogPropertieSetters()
        {
            // act
            var personExcludePropertySetters = new PersonExcludePropertySetters
            {
                Name = Guid.NewGuid().ToString()
            };
            var name = personExcludePropertySetters.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we only hit the Entry, Success and Exit methods for the instance and static constructor and property getter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropertieGettersFlag_ShouldNotLogPropetieGetters()
        {
            // act
            var personExcludePropertyGetters = new PersonExcludePropertyGetters
            {
                Name = Guid.NewGuid().ToString()
            };
            var name = personExcludePropertyGetters.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we only hit the Entry, Success and Exit methods for the instance constructor and property setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludeConstructorsFlag_ShouldNotLogConstructors()
        {
            // act
            var personExcludeConstructors = new PersonExcludeConstructors
            {
                Name = Guid.NewGuid().ToString()
            };
            var name = personExcludeConstructors.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we only hit the Entry, Success and Exit methods for the property getter and setter");
        }

        [Test]
        public void WhenAppliedToClassWithExcludePropertiesAndConstructorsFlag_ShouldNotLogPropertiesAndConstructor()
        {
            // act
            var personExcludePropertyConstructors = new PersonExcludePropertyConstructors
            {
                Name = Guid.NewGuid().ToString()
            };
            var name = personExcludePropertyConstructors.Name;

            // assert
            _logger.DebugCallCount.Should()
                .Be(0, "because we do not hit the Debug method for constructors and properties");
        }
    }
}