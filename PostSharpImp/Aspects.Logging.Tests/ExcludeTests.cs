namespace Aspects.Logging.Tests
{
    using System;

    using Aspects.Logging.Configuration.Abstract;

    using Commons.Dummies;
    using FluentAssertions;

    using Moq;

    using NUnit.Framework;
    using Utilities;

    /// <summary>
    /// The exclude tests.
    /// </summary>
    [TestFixture]
    public class ExcludeTests
    {
        /// <summary>
        /// The _logger.
        /// </summary>
        private MockLogger _logger;

        /// <summary>
        /// The initialize logger and aspect.
        /// </summary>
        [SetUp]
        public void InitializeLoggerAndAspect()
        {
            _logger = new MockLogger();
            LogAttribute.Logger = _logger;

            Mock<IConfigurationProvider> mock = new Mock<IConfigurationProvider>();
            mock.Setup(provider => provider.ShouldLog(It.IsAny<LogAttribute>())).Returns(true);
            LogAttribute.ConfigurationProvider = mock.Object;
        }

        /// <summary>
        /// The when applied to class with no exclude should log property and constructor.
        /// </summary>
        [Test]        
        public void WhenAppliedToClassWithNoExcludeShouldLogPropertyAndConstructor()
        {
            // act
            Person person = new Person
            {
                Name = Guid.NewGuid().ToString()
            };
            person.Name.Should().NotBeNullOrWhiteSpace();

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we hit the Entry, Success and Exit methods for both constructor and property");
        }

        /// <summary>
        /// The when applied to class with exclude property flag should not log property.
        /// </summary>
        [Test]
        public void WhenAppliedToClassWithExcludePropertyFlagShouldNotLogProperty()
        {
            // act
            PersonExcludeProperty personExcludeProperty = new PersonExcludeProperty
            {
                Name = Guid.NewGuid().ToString()
            };
            personExcludeProperty.Name.Should().NotBeNullOrWhiteSpace();

            // assert
            _logger.DebugCallCount.Should()
                .Be(3, "because we only hit the Entry, Success and Exit methods for the constructor");
        }

        /// <summary>
        /// The when applied to class with exclude instance constructor flag should not log instance constructor.
        /// </summary>
        [Test]
        public void WhenAppliedToClassWithExcludeInstanceConstructorFlagShouldNotLogInstanceConstructor()
        {
            // act
            PersonExcludeInstanceConstructor personExcludeInstanceConstructor = new PersonExcludeInstanceConstructor
            {
                Name = Guid.NewGuid().ToString()
            };
            personExcludeInstanceConstructor.Name.Should().NotBeNullOrWhiteSpace();

            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we only hit the Entry, Success and Exit methods for the property getter and setter");
        }

        /// <summary>
        /// The when applied to class with exclude static constructor flag should not log static constructor.
        /// </summary>
        [Test]
        public void WhenAppliedToClassWithExcludeStaticConstructorFlagShouldNotLogStaticConstructor()
        {
            // act
            PersonExcludeStaticConstructor personExcludeStaticConstructor = new PersonExcludeStaticConstructor
            {
                Name = Guid.NewGuid().ToString()
            };
            personExcludeStaticConstructor.Name.Should().NotBeNullOrWhiteSpace();

            // assert
            _logger.DebugCallCount.Should()
                .Be(9, "because we only hit the Entry, Success and Exit methods for the instance constructor and property getter and setter");
        }

        /// <summary>
        /// The when applied to class with exclude propertie getters flag should not log propriety getters.
        /// </summary>
        [Test]
        public void WhenAppliedToClassWithExcludePropertieGettersFlagShouldNotLogProprietyGetters()
        {
            // act
            PersonExcludePropertyGetters personExcludePropertyGetters = new PersonExcludePropertyGetters
            {
                Name = Guid.NewGuid().ToString()
            };
            personExcludePropertyGetters.Name.Should().NotBeNullOrWhiteSpace();

            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we only hit the Entry, Success and Exit methods for the instance constructor and property setter");
        }

        /// <summary>
        /// The when applied to class with exclude constructors flag should not log constructors.
        /// </summary>
        [Test]
        public void WhenAppliedToClassWithExcludeConstructorsFlagShouldNotLogConstructors()
        {
            // act
            PersonExcludeConstructors personExcludeConstructors = new PersonExcludeConstructors
            {
                Name = Guid.NewGuid().ToString()
            };
            personExcludeConstructors.Name.Should().NotBeNullOrWhiteSpace();

            // assert
            _logger.DebugCallCount.Should()
                .Be(6, "because we only hit the Entry, Success and Exit methods for the property getter and setter");
        }

        /// <summary>
        /// The when applied to class with exclude properties and constructors flag should not log properties and constructor.
        /// </summary>
        [Test]
        public void WhenAppliedToClassWithExcludePropertiesAndConstructorsFlagShouldNotLogPropertiesAndConstructor()
        {
            // act
            PersonExcludePropertyConstructors personExcludePropertyConstructors = new PersonExcludePropertyConstructors
            {
                Name = Guid.NewGuid().ToString()
            };
            personExcludePropertyConstructors.Name.Should().NotBeNullOrWhiteSpace();

            // assert
            _logger.DebugCallCount.Should()
                .Be(0, "because we do not hit the Debug method for constructors and properties");
        }
    }
}