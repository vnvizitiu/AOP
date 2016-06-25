namespace Aspects.Logging.Tests.Extensions
{
    using System;
    using Commons.Dummies;
    using FluentAssertions;
    using Logging.Extensions;
    using NUnit.Framework;

    /// <summary>
    /// The formattable object extension tests.
    /// </summary>
    [TestFixture]
    public class FormattableObjectExtensionTests
    {
        /// <summary>
        /// The when proving a custom format should return values for that format.
        /// </summary>
        [Test]
        public void WhenProvingACustomFormatShouldReturnValuesForThatFormat()
        {
            const string Teststring = "Andrei Ignat";
            FormatableObject formatableObject = new FormatableObject
            {
                StringProperty = Teststring
            };
            string str = formatableObject.ToString("{StringProperty}");
            str.Should().Be(Teststring, "because we are using a format string to get the value of the 'StringProperty' property");
        }

        /// <summary>
        /// The when proving a custom number format should return values for that format.
        /// </summary>
        [Test]
        public void WhenProvingACustomNumberFormatShouldReturnValuesForThatFormat()
        {
            const double DoubleProperty = 13.8;
            FormatableObject formatableObject = new FormatableObject
            {
                DoubleProperty = DoubleProperty
            };
            string str = formatableObject.ToString("{DoubleProperty:F}");
            str.Should().Be(DoubleProperty.ToString("F"), "because we are using a format string to get the value of the 'DoubleProperty' property with a format");
        }

        /// <summary>
        /// The when proving no format should return the provided format.
        /// </summary>
        [Test]
        public void WhenProvingNoFormatShouldReturnTheProvidedFormat()
        {
            const string Teststring = "TestString";
            FormatableObject person = new FormatableObject();
            string str = person.ToString(Teststring);
            str.Should().Be(Teststring, "because because the provided string had no property declared");
        }

        /// <summary>
        /// The when proving non existing property should return the format.
        /// </summary>
        [Test]
        public void WhenProvingNonExistingPropertyShouldReturnTheFormat()
        {
            const string Teststring = "{TestString}";
            FormatableObject formatableObject = new FormatableObject();
            string str = formatableObject.ToString(Teststring);
            str.Should().Be(Teststring, "because because the provided string had no property declared");
        }

        /// <summary>
        /// The when providing a field should return the format.
        /// </summary>
        [Test]
        public void WhenProvidingAFieldShouldReturnTheFormat()
        {
            const string Intfield = "{IntField}";
            const int TestInt = 15;
            FormatableObject formatableObject = new FormatableObject
            {
                IntField = 15
            };
            string str = formatableObject.ToString(Intfield);
            str.Should().Be(TestInt.ToString(), "because because the provided string had no property declared");
        }

        /// <summary>
        /// The when providing a null instance should throw exception.
        /// </summary>
        [Test]
        public void WhenProvidingANullInstanceShouldThrowException()
        {
            try
            {
                const string Teststring = "{TestString}";
                ((FormatableObject)null).ToString(Teststring);

                Assert.Fail("Should not hit this because we're supposed to throw an exception");
            }
            catch (Exception e)
            {
                e.Should().BeOfType<ArgumentNullException>();
            }
        }
    }
}