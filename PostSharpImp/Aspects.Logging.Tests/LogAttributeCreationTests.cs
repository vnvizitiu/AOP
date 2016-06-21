using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Aspects.Logging.Tests
{
    [TestFixture]
    public class LogAttributeCreationTests
    {
        [Test]
        public void WhenCreatingALogAttribut_ShoudlNotBeNull()
        {
            LogAttribute attribute = new LogAttribute();

            attribute.Should().NotBeNull();
        }
    }
}
