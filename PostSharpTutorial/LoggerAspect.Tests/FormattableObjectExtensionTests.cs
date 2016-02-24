using FluentAssertions;
using LoggerAspect;
using LoggerAspect.Extensions;
using LoggerAspect.Tests.Dummies;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class FormattableObjectExtensionTests
    {
        [Test]
        public void WhenProvingACustomFormat_ShouldReturnValuesForThatFormat()
        {
            string name = "Andrei Ignat";
            var p = new Person();
            p.Name = name;
            var str = p.ToString("{Name}");
            str.Should().Be(name, "because we are using a format string to get the value of the 'Name' property");            

        }
    }
}
