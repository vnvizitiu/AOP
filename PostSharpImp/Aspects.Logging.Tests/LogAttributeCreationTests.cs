namespace Aspects.Logging.Tests
{
    using FluentAssertions;

    using NUnit.Framework;

    /// <summary>
    /// The log attribute creation tests.
    /// </summary>
    [TestFixture]

    // ReSharper disable once TestFileNameWarning
    public class LogAttributeCreationTests
    {
        /// <summary>
        /// The when creating a log attribute should not be null.
        /// </summary>
        [Test]
        public void WhenCreatingALogAttributeShouldNotBeNull()
        {
            LogAttribute sut = new LogAttribute();
            sut.Should().NotBeNull();
        }
    }
}
