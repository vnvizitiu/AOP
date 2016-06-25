namespace Aspects.Logging.Tests.Helpers
{
    using System;
    using FluentAssertions;
    using Logging.Helpers;
    using NUnit.Framework;
    using PostSharp.Aspects;

    /// <summary>
    /// The message formatter tests.
    /// </summary>
    [TestFixture]
    public class MessageFormatterTests
    {
        /// <summary>
        /// The test 1.
        /// </summary>
        [Test]
        public void Test1()
        {
            string result = null;
            try
            {
                result = MessageFormatter.FormatMessage(
                    null, 
                    new MethodExecutionArgs(new object(), Arguments.Empty), 
                    string.Empty, 
                    null);
            }
            catch (Exception exception)
            {
                exception.Should().BeOfType<ArgumentNullException>("because we passed in a null logging info");
            }

            result.Should().BeNullOrWhiteSpace();
        }
    }
}