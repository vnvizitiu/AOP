using System;
using FluentAssertions;
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
        public void WhenAppliedToClassWithNoExclude_ShouldLogProperty()
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

    [LoggingAspect]
    public class Person
    {
        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.Properties)]
    public class PersonExcludeProperty
    {
        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.InstanceConstructors)]
    public class PersonExcludeInstanceConstructor
    {
        public PersonExcludeInstanceConstructor()
        {
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.StaticConstructor)]
    public class PersonExcludeStaticConstructor
    {
        static PersonExcludeStaticConstructor()
        {

        }


        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.PropertySetters)]
    public class PersonExcludePropertySetters
    {
        static PersonExcludePropertySetters()
        {

        }

        public PersonExcludePropertySetters()
        {
        }

        public PersonExcludePropertySetters(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.PropertyGetters)]
    public class PersonExcludePropertyGetters
    {
        static PersonExcludePropertyGetters()
        {

        }

        public PersonExcludePropertyGetters()
        {
        }

        public PersonExcludePropertyGetters(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.Constructors)]
    public class PersonExcludeConstructors
    {
        static PersonExcludeConstructors()
        {

        }

        public PersonExcludeConstructors()
        {
        }

        public PersonExcludeConstructors(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    [LoggingAspect(Exclude = ExclusionFlags.Properties | ExclusionFlags.Constructors)]
    public class PersonExcludePropertyConstructors
    {
        static PersonExcludePropertyConstructors()
        {

        }

        public PersonExcludePropertyConstructors()
        {
        }

        public PersonExcludePropertyConstructors(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
