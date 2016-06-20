using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Aspects.Logging.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class LogAspectSectionTests
    {
        [Test]
        public void OpenLogAspectConfiguration_ShouldReturnAValidConfig()
        {
            // arrange
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location) + ".config");
            File.Exists(path).Should().BeTrue("because the file needs to exist");

            // act
            var config = LogAspectConfig.Open(path);

            // assert
            config.Should().NotBeNull("because a valid output path has been provided");
            File.ReadAllText(path).Contains("section").Should().BeTrue("because we added the section at runtime");
        }

        [Test]
        public void OpenLogAspectConfigurationAndSave_ShouldReturnAValidConfig()
        {
            // arrange
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(Assembly.GetExecutingAssembly().Location));
            File.Exists(path).Should().BeTrue("because the file needs to exist");

            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(path);
            configuration.Sections.Remove("LogAspectConfig");
            configuration.Save();

            // act
            var config = LogAspectConfig.Open(path);
            config.Save();

            // assert
            config.Should().NotBeNull("because a valid output path has been provided");
            File.ReadAllText(path+".config").Contains("tags").Should().BeTrue("because we added the section at runtime after the save");
        }
    }
}