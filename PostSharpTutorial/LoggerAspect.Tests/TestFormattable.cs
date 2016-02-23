using LoggerAspect;
using NUnit.Framework;

namespace LoggerAspect.Tests
{
    [TestFixture]
    public class TestFormattable
    {
        [Test]
        public void TestWithProperties()
        {
            string site = "http://msprogrammer.serviciipeweb.ro/";
            var p = new Person();
            p.Website = site;
            var str = p.ToString("{Website}");
            Assert.AreEqual(str, site);

        }
    }
}
