namespace Aspects.Logging.Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using FluentAssertions;
    using Logging.Configuration.Infrastructure;
    using NUnit.Framework;

    [TestFixture]
    public class LogAspectSectionTests
    {
        /// <summary>
        /// The open log aspect configuration should return a valid config.
        /// </summary>
        [Test]
        public void OpenLogAspectConfigurationShouldReturnAValidConfig()
        {
            // arrange
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location) + ".config");
            File.Exists(path).Should().BeTrue("because the file needs to exist");

            // act
            LogAspectConfig config = LogAspectConfig.Open(path);

            // assert
            config.Should().NotBeNull("because a valid output path has been provided");
            File.ReadAllText(path).Contains("section").Should().BeTrue("because we added the section at runtime");
        }
    }
}