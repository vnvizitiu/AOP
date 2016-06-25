namespace Aspects.Logging.Tests.Configuration.Concrete
{
    using System;
    using System.Configuration;
    using FluentAssertions;
    using Logging.Configuration.Abstract;
    using Logging.Configuration.Concrete;
    using Logging.Configuration.Infrastructure;
    using Logging.Loggers;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// The config file configuration provider tests.
    /// </summary>
    [TestFixture]
    public class ConfigFileConfigurationProviderTests
    {
        /// <summary>
        /// The when no config mock is provided should throw null reference exception.
        /// </summary>
        [Test]
        public void WhenNoConfigMockIsProvidedShouldReturnNullLogger()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            ILogger result = sut.GetLogger();
            result.Should()
                .BeOfType<NullLogger>("because we default to the null logger is the config file cannot be read");
        }

        /// <summary>
        /// The when passing in a configuration with custom logger and the console logger should throw configuration error exception.
        /// </summary>
        [Test]
        public void WhenPassingInAConfigurationWithCustomLoggerAndTheConsoleLoggerShouldThrowConfigurationErrorException()
        {
            ILogger result = null;

            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.Logger).Returns("NullLogger");
            mock.Setup(source => source.UseConsoleLogger).Returns(true);
            mock.Setup(source => source.IsEnabled).Returns(true);
            sut.ConfigFileSource = mock.Object;
            try
            {
                result = sut.GetLogger();
            }
            catch (Exception exception)
            {
                exception.Should()
                    .BeOfType<ConfigurationErrorsException>("because we set up a configuration that has both a custom logger and the flag to use the console logger");
            }

            result.Should().BeNull("because we threw an exception");
        }

        /// <summary>
        /// The when passing in a configuration with console logger on should return connsole logger.
        /// </summary>
        [Test]
        public void WhenPassingInAConfigurationWithConsoleLoggerOnShouldReturnConnsoleLogger()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.UseConsoleLogger).Returns(true);
            mock.Setup(source => source.IsEnabled).Returns(true);
            sut.ConfigFileSource = mock.Object;

            ILogger result = sut.GetLogger();

            result.Should().BeOfType<ConsoleLogger>("because we set up a configuration that uses the console logger");
        }

        /// <summary>
        /// The when passing in a configuration with a custom logger should custom logger.
        /// </summary>
        [Test]
        public void WhenPassingInAConfigurationWithACustomLoggerShouldCustomLogger()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.Logger).Returns(typeof(ConsoleLogger).ToString);
            mock.Setup(source => source.UseConsoleLogger).Returns(false);
            mock.Setup(source => source.IsEnabled).Returns(true);
            sut.ConfigFileSource = mock.Object;

            ILogger result = sut.GetLogger();

            result.Should().BeOfType<ConsoleLogger>("because we set up a configuration that uses the console logger");
        }

        /// <summary>
        /// The when passing in a configuration with no viable loggers should throw exception.
        /// </summary>
        [Test]
        public void WhenPassingInAConfigurationWithNoViableLoggersShouldThrowException()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.Logger).Returns(string.Empty);
            mock.Setup(source => source.UseConsoleLogger).Returns(false);
            mock.Setup(source => source.IsEnabled).Returns(true);
            sut.ConfigFileSource = mock.Object;

            ILogger result = null;
            try
            {
                result = sut.GetLogger();
            }
            catch (Exception e)
            {
                e.Should().BeOfType<ConfigurationErrorsException>();
            }

            result.Should().BeNull("Because an exception has been thrown");
        }

        /// <summary>
        /// The when passing in a configuration with an invalid logger should throw exception.
        /// </summary>
        [Test]
        public void WhenPassingInAConfigurationWithAnInvalidLoggerShouldThrowException()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.Logger).Returns("NonViableType");
            mock.Setup(source => source.UseConsoleLogger).Returns(false);
            mock.Setup(source => source.IsEnabled).Returns(true);
            sut.ConfigFileSource = mock.Object;

            ILogger result = null;
            try
            {
                result = sut.GetLogger();
            }
            catch (Exception e)
            {
                e.Should().BeOfType<ConfigurationErrorsException>();
            }

            result.Should().BeNull("Because an exception has been thrown");
        }

        /// <summary>
        /// The when passing a disabled config should return false.
        /// </summary>
        [Test]
        public void WhenPassingNoConfigShouldReturnFalse()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            LogAttribute attribute = new LogAttribute();

            bool shouldLog = sut.ShouldLog(attribute);

            shouldLog.Should().Be(false);
        }

        /// <summary>
        /// The when passing a disabled config should return false.
        /// </summary>
        [Test]
        public void WhenPassingADisabledConfigShouldReturnFalse()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.IsEnabled).Returns(false);
            sut.ConfigFileSource = mock.Object;

            LogAttribute attribute = new LogAttribute();

            bool shouldLog = sut.ShouldLog(attribute);

            shouldLog.Should().Be(false);
        }

        /// <summary>
        /// The when passing an enabled config should return true.
        /// </summary>
        [Test]
        public void WhenPassingAnEnabledConfigShouldReturnTrue()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.IsEnabled).Returns(true);
            mock.Setup(source => source.Tags).Returns(new TagCollection());
            sut.ConfigFileSource = mock.Object;

            LogAttribute attribute = new LogAttribute();

            bool shouldLog = sut.ShouldLog(attribute);

            shouldLog.Should().Be(true);
        }

        /// <summary>
        /// The when passing an enabled config with a nominal tag should return true.
        /// </summary>
        [Test]
        public void WhenPassingAnEnabledConfigWithANominalTagShouldReturnTrue()
        {
            ConfigFileConfigurationProvider sut = new ConfigFileConfigurationProvider();
            Mock<IConfigFileSource> mock = new Mock<IConfigFileSource>();
            mock.Setup(source => source.IsEnabled).Returns(true);
            TagCollection tagCollection = new TagCollection();
            mock.Setup(source => source.Tags).Returns(tagCollection);
            sut.ConfigFileSource = mock.Object;

            LogAttribute attribute = new LogAttribute();

            bool shouldLog = sut.ShouldLog(attribute);

            shouldLog.Should().Be(true);
        }
    }
}