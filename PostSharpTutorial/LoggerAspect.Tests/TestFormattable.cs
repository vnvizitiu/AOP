using LoggerAspect;
using LoggerAspect.Extensions;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class TestFormattable
    {
        [Test]
        public void TestWithProperties()
        {
            string site = "Andrei Ignat";
            var p = new Person();
            p.Name = site;
            var str = p.ToString("{Name}");
            Assert.AreEqual(str, site);

        }
    }
}
