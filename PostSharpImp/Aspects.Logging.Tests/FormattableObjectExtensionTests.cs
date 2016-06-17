using Aspects.Logging.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class FormattableObjectExtensionTests
    {
        [Test]
        public void WhenProvingACustomFormat_ShouldReturnValuesForThatFormat()
        {
            const string name = "Andrei Ignat";
            Person person = new Person
            {
                Name = name
            };
            string str = person.ToString("{Name}");
            str.Should().Be(name, "because we are using a format string to get the value of the 'Name' property");
        }

        [Test]
        public void WhenProvingACustomNumberFormat_ShouldReturnValuesForThatFormat()
        {
            const double balance = 13.8;
            Person person = new Person
            {
                Balance = balance
            };
            string str = person.ToString("{Balance:F}");
            str.Should().Be(balance.ToString("F"), "because we are using a format string to get the value of the 'Balance' property with a format");
        }

        [Test]
        public void WhenProvingNoFormat_ShouldReturnTheProvidedFormat()
        {
            const string name = "TestString";
            Person person = new Person();
            string str = person.ToString(name);
            str.Should().Be(name, "because because the provided string had no property declared");
        }

        [Test]
        public void WhenProvingNonExistingProperty_ShouldReturnTheFormat()
        {
            const string name = "{TestString}";
            Person person = new Person();
            string str = person.ToString(name);
            str.Should().Be(name, "because because the provided string had no property declared");
        }
    }
}